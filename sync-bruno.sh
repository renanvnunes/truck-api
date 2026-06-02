#!/usr/bin/env bash

set -euo pipefail

SPEC_URL="http://localhost:5053/openapi/v1.json"
BRUNO_DIR="$(dirname "$0")/BrunoApi"

STATUS_TEXT() {
  case $1 in
    200) echo "OK" ;; 201) echo "Created" ;; 204) echo "No Content" ;;
    400) echo "Bad Request" ;; 401) echo "Unauthorized" ;;
    403) echo "Forbidden" ;; 404) echo "Not Found" ;;
    429) echo "Too Many Requests" ;; *) echo "Response" ;;
  esac
}

echo "Buscando spec em $SPEC_URL..."
if ! SPEC=$(curl -sf "$SPEC_URL"); then
  echo "Erro: nao foi possivel buscar a spec. A API esta rodando?"
  exit 1
fi

ADDED=0

PATHS=$(echo "$SPEC" | jq -r '.paths | keys[]')

for ROUTE_PATH in $PATHS; do
  METHODS=$(echo "$SPEC" | jq -r ".paths[\"$ROUTE_PATH\"] | keys[]")

  for METHOD in $METHODS; do
    OP=$(echo "$SPEC" | jq ".paths[\"$ROUTE_PATH\"][\"$METHOD\"]")

    TAG=$(echo "$OP" | jq -r '.tags[0] // "Default"')
    NAME=$(echo "$OP" | jq -r ".summary // \"${METHOD^^} ${ROUTE_PATH}\"")
    DESCRIPTION=$(echo "$OP" | jq -r '.description // ""')
    SAFE_NAME=$(echo "$NAME" | tr '/\\:*?"<>|' '-' | xargs)

    FOLDER_PATH="$BRUNO_DIR/$TAG"
    FILE_PATH="$FOLDER_PATH/$SAFE_NAME.yml"

    [ -f "$FILE_PATH" ] && continue

    if [ ! -d "$FOLDER_PATH" ]; then
      mkdir -p "$FOLDER_PATH"
      SEQ_FOLDER=$(find "$BRUNO_DIR" -mindepth 1 -maxdepth 1 -type d | wc -l)
      cat > "$FOLDER_PATH/folder.yml" <<EOF
info:
  name: $TAG
  type: folder
  seq: $SEQ_FOLDER

request:
  auth: inherit
EOF
    fi

    SEQ=$(find "$FOLDER_PATH" -maxdepth 1 -name "*.yml" ! -name "folder.yml" | wc -l)
    SEQ=$((SEQ + 1))

    URL="{{baseUrl}}${ROUTE_PATH}"
    URL=$(echo "$URL" | sed 's/{\([^}]*\)}/{{\1}}/g')

    BODY_SCHEMA=$(echo "$OP" | jq '.requestBody.content["application/json"].schema // null')
    if [ "$BODY_SCHEMA" != "null" ]; then
      BODY_EXAMPLE=$(echo "$SPEC" | jq --argjson schema "$BODY_SCHEMA" '
        def example($s):
          if $s == null then null
          elif $s["$ref"] != null then
            example(.components.schemas[$s["$ref"] | split("/") | last])
          elif $s.type == "object" then
            ($s.properties // {}) | with_entries(.value = example(.value))
          elif $s.type == "array" then []
          elif $s.type == "string" then ""
          elif ($s.type == "integer" or $s.type == "number") then 0
          elif $s.type == "boolean" then false
          else null
          end;
        example($schema)
      ')
      BODY_JSON=$(echo "$BODY_EXAMPLE" | jq '.')
      BODY_BLOCK=$(echo "$BODY_JSON" | sed 's/^/      /')
      BODY_SECTION="  body:
    type: json
    data: |-
$BODY_BLOCK"
    else
      BODY_SECTION=""
      BODY_JSON="null"
    fi

    EXAMPLES_BLOCK=""
    RESPONSES=$(echo "$OP" | jq -r '.responses | keys[]' 2>/dev/null || true)
    if [ -n "$RESPONSES" ]; then
      EXAMPLES_BLOCK=$'\nexamples:'
      for STATUS_CODE in $RESPONSES; do
        STATUS_INT=$(echo "$STATUS_CODE" | tr -d '"')
        STATUS_TXT=$(STATUS_TEXT "$STATUS_INT")

        RESP_SCHEMA=$(echo "$OP" | jq ".responses[\"$STATUS_CODE\"].content[\"application/json\"].schema // null")
        if [ "$RESP_SCHEMA" != "null" ]; then
          RESP_EXAMPLE=$(echo "$SPEC" | jq --argjson schema "$RESP_SCHEMA" '
            def example($s):
              if $s == null then null
              elif $s["$ref"] != null then
                example(.components.schemas[$s["$ref"] | split("/") | last])
              elif $s.type == "object" then
                ($s.properties // {}) | with_entries(.value = example(.value))
              elif $s.type == "array" then []
              elif $s.type == "string" then ""
              elif ($s.type == "integer" or $s.type == "number") then 0
              elif $s.type == "boolean" then false
              else null
              end;
            example($schema)
          ')
          RESP_JSON=$(echo "$RESP_EXAMPLE" | jq '.')
          RESP_BODY_BLOCK=$(echo "$RESP_JSON" | sed 's/^/          /')
          RESP_BODY_SECTION="      body:
        type: json
        data: |-
$RESP_BODY_BLOCK"
        else
          RESP_BODY_SECTION=""
        fi

        if [ "$BODY_JSON" != "null" ]; then
          REQ_BODY_BLOCK=$(echo "$BODY_JSON" | sed 's/^/          /')
          REQ_BODY_IN_EXAMPLE="      body:
        type: json
        data: |-
$REQ_BODY_BLOCK"
        else
          REQ_BODY_IN_EXAMPLE=""
        fi

        EXAMPLES_BLOCK="$EXAMPLES_BLOCK
  - name: $STATUS_INT Response
    description: $STATUS_TXT
    request:
      url: \"$URL\"
      method: ${METHOD^^}
$REQ_BODY_IN_EXAMPLE
    response:
      status: $STATUS_INT
      statusText: $STATUS_TXT
      headers:
        - name: Content-Type
          value: application/json
$RESP_BODY_SECTION"
      done
    fi

    DOCS_BLOCK=""
    [ -n "$DESCRIPTION" ] && DOCS_BLOCK=$'\n'"docs: $DESCRIPTION"

    {
      echo "info:"
      echo "  name: $SAFE_NAME"
      echo "  type: http"
      echo "  seq: $SEQ"
      echo "  tags:"
      echo "    - $TAG"
      echo ""
      echo "http:"
      echo "  method: ${METHOD^^}"
      echo "  url: \"$URL\""
      [ -n "$BODY_SECTION" ] && echo "$BODY_SECTION"
      echo "  auth: inherit"
      echo ""
      echo "settings:"
      echo "  encodeUrl: true"
      echo "  timeout: 0"
      echo "  followRedirects: true"
      echo "  maxRedirects: 5"
      [ -n "$EXAMPLES_BLOCK" ] && echo "$EXAMPLES_BLOCK"
      [ -n "$DOCS_BLOCK" ] && echo "$DOCS_BLOCK"
    } > "$FILE_PATH"

    echo "Adicionado: $TAG/$SAFE_NAME"
    ADDED=$((ADDED + 1))
  done
done

if [ "$ADDED" -eq 0 ]; then
  echo "Tudo atualizado, nenhuma rota nova encontrada."
else
  echo "Concluido! $ADDED nova(s) rota(s) adicionada(s)."
fi

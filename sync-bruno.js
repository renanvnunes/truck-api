#!/usr/bin/env node

const fs = require('fs');
const path = require('path');
const http = require('http');

const SPEC_URL = 'http://localhost:5053/openapi/v1.json';
const BRUNO_DIR = path.join(__dirname, 'BrunoApi');

function fetchJson(url) {
  return new Promise((resolve, reject) => {
    http.get(url, (res) => {
      let data = '';
      res.on('data', (chunk) => (data += chunk));
      res.on('end', () => resolve(JSON.parse(data)));
    }).on('error', reject);
  });
}

function schemaToExample(schema, schemas, depth = 0) {
  if (!schema || depth > 5) return null;
  if (schema.$ref) {
    const name = schema.$ref.split('/').pop();
    return schemaToExample(schemas[name], schemas, depth + 1);
  }
  if (schema.allOf) return schemaToExample(schema.allOf[0], schemas, depth + 1);
  switch (schema.type) {
    case 'object': {
      const obj = {};
      for (const [key, prop] of Object.entries(schema.properties || {}))
        obj[key] = schemaToExample(prop, schemas, depth + 1);
      return obj;
    }
    case 'array': return [];
    case 'string': return '';
    case 'integer':
    case 'number': return 0;
    case 'boolean': return false;
    default: return null;
  }
}

function indentJson(obj, spaces) {
  return JSON.stringify(obj, null, 2)
    .split('\n')
    .map((l) => ' '.repeat(spaces) + l)
    .join('\n');
}

const STATUS_TEXT = {
  200: 'OK', 201: 'Created', 204: 'No Content',
  400: 'Bad Request', 401: 'Unauthorized',
  403: 'Forbidden', 404: 'Not Found', 429: 'Too Many Requests',
};

function buildFile({ name, method, url, tag, seq, bodyExample, responses, description }) {
  const m = method.toUpperCase();

  const bodyBlock =
    bodyExample !== null
      ? `  body:\n    type: json\n    data: |-\n${indentJson(bodyExample, 6)}\n`
      : '';

  const exampleReqBody =
    bodyExample !== null
      ? `      body:\n        type: json\n        data: |-\n${indentJson(bodyExample, 10)}\n`
      : '';

  let examplesBlock = '';
  if (responses.length > 0) {
    examplesBlock = '\nexamples:\n';
    for (const res of responses) {
      const resBodyBlock =
        res.body !== null
          ? `      body:\n        type: json\n        data: |-\n${indentJson(res.body, 10)}\n`
          : '';
      examplesBlock +=
        `  - name: ${res.status} Response\n` +
        `    description: ${res.statusText}\n` +
        `    request:\n` +
        `      url: "${url}"\n` +
        `      method: ${m}\n` +
        exampleReqBody +
        `    response:\n` +
        `      status: ${res.status}\n` +
        `      statusText: ${res.statusText}\n` +
        `      headers:\n` +
        `        - name: Content-Type\n` +
        `          value: application/json\n` +
        resBodyBlock;
    }
  }

  const docsBlock = description ? `\ndocs: ${description}\n` : '';

  return (
    `info:\n  name: ${name}\n  type: http\n  seq: ${seq}\n  tags:\n    - ${tag}\n\n` +
    `http:\n  method: ${m}\n  url: "${url}"\n${bodyBlock}  auth: inherit\n\n` +
    `settings:\n  encodeUrl: true\n  timeout: 0\n  followRedirects: true\n  maxRedirects: 5\n` +
    examplesBlock +
    docsBlock
  );
}

async function main() {
  let spec;
  try {
    spec = await fetchJson(SPEC_URL);
  } catch {
    console.error('Erro: nao foi possivel buscar a spec. A API esta rodando em', SPEC_URL, '?');
    process.exit(1);
  }

  const schemas = spec.components?.schemas || {};
  let added = 0;

  for (const [routePath, pathItem] of Object.entries(spec.paths || {})) {
    for (const method of ['get', 'post', 'put', 'patch', 'delete']) {
      const op = pathItem[method];
      if (!op) continue;

      const tag = op.tags?.[0] || 'Default';
      const name = op.summary || `${method.toUpperCase()} ${routePath}`;
      const safeName = name.replace(/[/\\:*?"<>|]/g, '-').trim();
      const description = op.description || '';
      const folderPath = path.join(BRUNO_DIR, tag);
      const filePath = path.join(folderPath, `${safeName}.yml`);

      if (fs.existsSync(filePath)) continue;

      if (!fs.existsSync(folderPath)) {
        fs.mkdirSync(folderPath, { recursive: true });
        const seq = fs.readdirSync(BRUNO_DIR).filter((f) =>
          fs.statSync(path.join(BRUNO_DIR, f)).isDirectory()
        ).length;
        fs.writeFileSync(
          path.join(folderPath, 'folder.yml'),
          `info:\n  name: ${tag}\n  type: folder\n  seq: ${seq}\n\nrequest:\n  auth: inherit\n`
        );
      }

      const seq = fs.readdirSync(folderPath).filter((f) => f !== 'folder.yml').length + 1;
      const url = `{{baseUrl}}${routePath}`.replace(/\{(\w+)\}/g, '{{$1}}');

      let bodyExample = null;
      const reqSchema = op.requestBody?.content?.['application/json']?.schema;
      if (reqSchema) bodyExample = schemaToExample(reqSchema, schemas);

      const responses = Object.entries(op.responses || {}).map(([code, resp]) => {
        const status = parseInt(code) || code;
        const statusText = STATUS_TEXT[status] || 'Response';
        const respSchema = resp.content?.['application/json']?.schema;
        const body = respSchema ? schemaToExample(respSchema, schemas) : null;
        return { status, statusText, body };
      });

      fs.writeFileSync(filePath, buildFile({ name: safeName, method, url, tag, seq, bodyExample, responses, description }));
      console.log(`Adicionado: ${tag}/${name}`);
      added++;
    }
  }

  console.log(added === 0 ? '\nTudo atualizado, nenhuma rota nova encontrada.' : `\nConcluido! ${added} nova(s) rota(s) adicionada(s).`);
}

main().catch((err) => {
  console.error('Erro:', err.message);
  process.exit(1);
});

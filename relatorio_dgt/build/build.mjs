import { readFileSync, writeFileSync } from 'node:fs';
import { marked } from 'marked';

const MD = process.argv[2];
const HTML_OUT = process.argv[3];

let md = readFileSync(MD, 'utf8');

// Substitui cada bloco ```mermaid por uma <figure><img> em ordem
let i = 0;
md = md.replace(/```mermaid\s*\r?\n[\s\S]*?```/g, () => {
  i++;
  return `<figure class="diagram">
  <img src="diagrams/d${i}.svg" alt="Diagrama ${i}" />
</figure>`;
});

const body = marked.parse(md);

const html = `<!DOCTYPE html>
<html lang="pt">
<head>
<meta charset="utf-8" />
<title>Relatório — Fluxo lógico da extração de parafusos do Tekla e inclusão no DGT</title>
<style>
  @page { size: A4; margin: 16mm 14mm; }
  :root { color-scheme: light; }
  * { box-sizing: border-box; }
  body {
    font-family: "Segoe UI", Arial, sans-serif;
    color: #1f2328;
    font-size: 12.5px;
    line-height: 1.55;
    margin: 0;
    max-width: 100%;
  }
  h1 { font-size: 22px; border-bottom: 3px solid #1f6feb; padding-bottom: 6px; margin-top: 0; }
  h2 { font-size: 16px; margin-top: 1.6em; border-bottom: 1px solid #d0d7de; padding-bottom: 3px; color: #0b3d91; }
  h3 { font-size: 13.5px; margin-top: 1.3em; color: #17508c; }
  h4 { font-size: 12.5px; margin-top: 1.1em; }
  a { color: #1f6feb; text-decoration: none; }
  code {
    font-family: "Cascadia Mono", Consolas, monospace;
    font-size: 11px;
    background: #f3f4f6;
    padding: 1px 4px;
    border-radius: 4px;
  }
  pre {
    background: #f6f8fa;
    border: 1px solid #d0d7de;
    border-radius: 6px;
    padding: 10px 12px;
    overflow-x: auto;
    white-space: pre-wrap;
    word-wrap: break-word;
  }
  pre code { background: none; padding: 0; }
  table { border-collapse: collapse; width: 100%; margin: 10px 0; }
  th, td { border: 1px solid #d0d7de; padding: 5px 8px; text-align: left; vertical-align: top; }
  th { background: #eef1f5; }
  blockquote { border-left: 4px solid #d0d7de; margin: 10px 0; padding: 2px 12px; color: #57606a; background: #fafbfc; }
  hr { border: none; border-top: 1px solid #d0d7de; margin: 1.4em 0; }
  figure.diagram {
    margin: 16px auto;
    text-align: center;
    page-break-inside: avoid;
  }
  figure.diagram img {
    max-width: 100%;
    height: auto;
    border: 1px solid #e1e4e8;
    border-radius: 6px;
    padding: 6px;
    background: #fff;
  }
  ul, ol { padding-left: 22px; }
  li { margin: 2px 0; }
</style>
</head>
<body>
${body}
</body>
</html>`;

writeFileSync(HTML_OUT, html, 'utf8');
console.log(`Wrote ${HTML_OUT} (${html.length} bytes) with ${i} diagram images`);

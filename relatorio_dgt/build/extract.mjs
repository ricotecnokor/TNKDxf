import { readFileSync, writeFileSync, mkdirSync } from 'node:fs';

const MD = process.argv[2];
const OUT = process.argv[3];

const md = readFileSync(MD, 'utf8');
mkdirSync(OUT, { recursive: true });

const re = /```mermaid\s*\r?\n([\s\S]*?)```/g;
let m, i = 0;
while ((m = re.exec(md)) !== null) {
  i++;
  const body = m[1].trim();
  writeFileSync(`${OUT}/d${i}.mmd`, body + '\n', 'utf8');
}
console.log(`Extracted ${i} mermaid blocks to ${OUT}`);

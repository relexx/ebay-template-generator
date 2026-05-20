/* global React */
/* ============================================================
   HTML generator — icons variant
   Block icons stored as Lucide names; emitted as a small SVG
   alongside the section title in the generated eBay HTML.
   ============================================================ */

function escapeHtml(s) {
  return String(s ?? '')
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');
}

function mdInline(s) {
  return escapeHtml(s)
    .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
    .replace(/(^|[^*])\*([^*]+)\*/g, '$1<em>$2</em>');
}

function mdParagraphs(s) {
  return String(s ?? '')
    .split(/\n\s*\n/)
    .map(p => `<p style="margin: 0 0 14px 0;">${mdInline(p).replace(/\n/g, '<br>')}</p>`)
    .join('');
}

function parseRows(raw) {
  return String(raw ?? '')
    .split(/\r?\n/)
    .map(l => l.trim())
    .filter(Boolean)
    .map(l => {
      const [k, ...rest] = l.split('|').map(s => s.trim());
      return { k: k ?? '', v: rest.join(' | ') };
    });
}

/* ----- Raw SVG path strings for emitted HTML ----- */
const SVG_RAW = {
  image:     '<rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="9" cy="9" r="2"/><path d="M21 15l-5-5L5 21"/>',
  fileText:  '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><path d="M14 2v6h6"/><path d="M8 13h8"/><path d="M8 17h6"/>',
  sparkles:  '<path d="M12 3l1.8 4.8L18 9.6l-4.2 1.8L12 16.2 10.2 11.4 6 9.6l4.2-1.8z"/><path d="M19 14l.8 2.2L22 17l-2.2.8L19 20l-.8-2.2L16 17l2.2-.8z"/>',
  settings:  '<circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/>',
  wrench:    '<path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"/>',
  box:       '<path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><path d="M3.27 6.96L12 12.01l8.73-5.05"/><path d="M12 22V12"/>',
  lightbulb: '<path d="M9 18h6"/><path d="M10 22h4"/><path d="M12 2a7 7 0 0 0-4 12.7c.5.5 1 1.3 1 2.3h6c0-1 .5-1.8 1-2.3A7 7 0 0 0 12 2z"/>',
  star:      '<path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77 5.82 21l1.18-6.88-5-4.87 6.91-1.01z"/>',
  target:    '<circle cx="12" cy="12" r="10"/><circle cx="12" cy="12" r="6"/><circle cx="12" cy="12" r="2"/>',
  barChart:  '<path d="M3 3v18h18"/><rect x="7" y="13" width="3" height="6"/><rect x="12" y="9" width="3" height="10"/><rect x="17" y="5" width="3" height="14"/>',
  search:    '<circle cx="11" cy="11" r="7"/><path d="M21 21l-4.35-4.35"/>',
  zap:       '<path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/>',
  hammer:    '<path d="M15 12l-8.5 8.5a2.12 2.12 0 0 1-3-3L12 9"/><path d="M17.6 6.4l4 4M11.8 6.4l-5 5"/>',
  ruler:     '<path d="M3 17l6-6 6 6 6-6"/><path d="M21 7v10H3V7"/>',
  palette:   '<circle cx="13.5" cy="6.5" r="1.5"/><circle cx="17.5" cy="10.5" r="1.5"/><circle cx="8.5" cy="7.5" r="1.5"/><circle cx="6.5" cy="12.5" r="1.5"/><path d="M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.554C21.965 6.012 17.461 2 12 2z"/>',
  pin:       '<path d="M12 2v10"/><path d="M9 12h6"/><path d="M9 12l3 10 3-10"/><circle cx="12" cy="6" r="3"/>',
};

function svgIcon(name, color = 'currentColor', size = 16) {
  const body = SVG_RAW[name];
  if (!body) return '';
  return `<svg width="${size}" height="${size}" viewBox="0 0 24 24" fill="none" stroke="${color}" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" style="vertical-align: -3px; margin-right: 8px;">${body}</svg>`;
}

function sectionTitle(block, colors) {
  if (!block.showTitle) return '';
  return `<h3 style="margin: 0 0 16px 0; color: ${colors.primary}; font-size: 17px; font-weight: 600; border-bottom: 2px solid ${colors.accent}; padding-bottom: 8px; display: inline-block;">
    ${svgIcon(block.icon, colors.accent)}${escapeHtml(block.title)}
  </h3>`;
}

function genHeader(c, colors) {
  return `<!-- Header -->
<table width="100%" cellpadding="0" cellspacing="0" border="0" style="background: ${colors.primary}; border-radius: 4px 4px 0 0; margin-bottom: 0;">
  <tr>
    <td style="padding: 28px 32px; text-align: center;">
      <div style="margin: 0; color: ${colors.accent}; font-size: 24px; font-weight: 600; letter-spacing: -0.02em;">
        ${escapeHtml(c.title)}
      </div>
      <div style="margin: 8px 0 0 0; color: #cccccc; font-size: 13px; font-family: ui-monospace, monospace;">
        ${escapeHtml(c.subtitle)}
      </div>
    </td>
  </tr>
</table>`;
}

function genImage(block, c, colors) {
  const url = c.img?.url || `https://placehold.co/600x400/${colors.primary.replace('#', '')}/${colors.accent.replace('#', '')}?text=${encodeURIComponent(block.title || 'Bild')}`;
  return `<!-- ${escapeHtml(block.title)} -->
<div style="text-align: center; padding: 24px 0; background: ${colors.bg};">
  ${block.showTitle ? `<h3 style="margin: 0 0 16px 0; color: ${colors.primary}; font-size: 18px;">${svgIcon(block.icon, colors.accent)}${escapeHtml(block.title)}</h3>` : ''}
  <img src="${escapeHtml(url)}" alt="${escapeHtml(block.title)}" style="max-width: ${block.maxWidth || 600}px; width: 100%; height: auto; border-radius: 4px;" />
</div>`;
}

function genDescription(block, c, colors) {
  return `<!-- ${escapeHtml(block.title)} -->
<div style="padding: 24px 32px; background: ${colors.bg};">
  ${sectionTitle(block, colors)}
  <div style="color: #2a2a2a; line-height: 1.65;">
    ${mdParagraphs(c.description?.text || '')}
  </div>
</div>`;
}

function genHighlights(block, c, colors) {
  const rows = parseRows(c.highlights?.raw);
  const pairs = [];
  for (let i = 0; i < rows.length; i += 2) {
    pairs.push(`<tr>
      <td style="vertical-align: top; width: 50%; padding: 10px 12px;">
        <div style="display: flex; gap: 8px;">
          <span style="color: ${colors.accent}; font-weight: 700;">▸</span>
          <div>
            <div style="font-weight: 600; color: ${colors.primary}; font-size: 14px;">${escapeHtml(rows[i].k)}</div>
            <div style="color: #555; font-size: 13px; margin-top: 2px;">${escapeHtml(rows[i].v)}</div>
          </div>
        </div>
      </td>
      ${rows[i+1] ? `<td style="vertical-align: top; width: 50%; padding: 10px 12px;">
        <div style="display: flex; gap: 8px;">
          <span style="color: ${colors.accent}; font-weight: 700;">▸</span>
          <div>
            <div style="font-weight: 600; color: ${colors.primary}; font-size: 14px;">${escapeHtml(rows[i+1].k)}</div>
            <div style="color: #555; font-size: 13px; margin-top: 2px;">${escapeHtml(rows[i+1].v)}</div>
          </div>
        </div>
      </td>` : '<td></td>'}
    </tr>`);
  }
  return `<!-- ${escapeHtml(block.title)} -->
<div style="padding: 24px 32px; background: ${colors.bg};">
  ${sectionTitle(block, colors)}
  <table width="100%" cellpadding="0" cellspacing="0" border="0">
    ${pairs.join('\n    ')}
  </table>
</div>`;
}

function genTechData(block, c, colors) {
  const rows = parseRows(c.techData?.raw);
  return `<!-- ${escapeHtml(block.title)} -->
<div style="padding: 24px 32px; background: ${colors.bg};">
  ${sectionTitle(block, colors)}
  <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border: 1px solid #e5e5e5; border-radius: 4px;">
    ${rows.map((r, i) => `<tr style="background: ${i % 2 ? '#fafafa' : '#ffffff'};">
      <td style="padding: 10px 14px; font-weight: 500; color: #2a2a2a; border-bottom: 1px solid #f0f0f0; width: 40%;">${escapeHtml(r.k)}</td>
      <td style="padding: 10px 14px; color: #555; border-bottom: 1px solid #f0f0f0;">${escapeHtml(r.v)}</td>
    </tr>`).join('\n    ')}
  </table>
</div>`;
}

function genCompatibility(block, c, colors) {
  const rows = parseRows(c.compatibility?.raw);
  return `<!-- ${escapeHtml(block.title)} -->
<div style="padding: 24px 32px; background: ${colors.bg};">
  ${sectionTitle(block, colors)}
  <table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
      ${rows.map(r => `<td style="vertical-align: top; padding: 8px;">
        <div style="border: 1px solid #e5e5e5; border-radius: 4px; padding: 14px;">
          <div style="font-weight: 600; color: ${colors.primary}; font-size: 14px; margin-bottom: 4px;">${escapeHtml(r.k)}</div>
          <div style="color: #555; font-size: 13px;">${escapeHtml(r.v)}</div>
        </div>
      </td>`).join('\n      ')}
    </tr>
  </table>
</div>`;
}

const BLOCK_GENERATORS = {
  bild: genImage,
  fließtext: genDescription,
  stichwortkarten: genHighlights,
  datentabelle: genTechData,
  featurekarten: genCompatibility,
};

function generateHTML(template, content) {
  const colors = template.colors;
  const headerOut = genHeader(content, colors);
  const blocksOut = template.blocks.map(b => {
    const gen = BLOCK_GENERATORS[b.type] || genDescription;
    return gen(b, content, colors);
  }).join('\n\n');

  return `<!-- eBay HTML-Template — generiert mit relexx Template Generator -->
<!-- Layout: ${escapeHtml(template.name)} -->

<meta name="viewport" content="width=device-width, initial-scale=1">

<div style="max-width: 760px; margin: 0 auto; font-family: 'Geist', -apple-system, system-ui, sans-serif; color: #1a1a1a; line-height: 1.6; font-size: 15px;">

  <!-- Mobile Summary (schema.org) -->
  <div vocab="https://schema.org/" typeof="Product" style="margin-bottom: 20px;">
    <span property="description">${escapeHtml(content.summary || '')}</span>
  </div>

${headerOut}

${blocksOut}

</div>
<!-- /Template -->`;
}

function syntaxHighlight(html) {
  const lines = html.split('\n');
  return lines.map((line, i) => {
    if (/^\s*<!--/.test(line)) {
      return <div key={i}><span className="code-comment">{line}</span></div>;
    }
    const parts = [];
    let key = 0;
    const re = /(<\/?)([a-zA-Z][a-zA-Z0-9-]*)|(\s[a-zA-Z-]+)=("[^"]*")/g;
    let lastIndex = 0;
    let match;
    while ((match = re.exec(line)) !== null) {
      if (match.index > lastIndex) {
        parts.push(<span key={key++}>{line.slice(lastIndex, match.index)}</span>);
      }
      if (match[2]) {
        parts.push(<span key={key++} className="code-tag">{match[1]}{match[2]}</span>);
      } else if (match[3]) {
        parts.push(<span key={key++} className="code-attr">{match[3]}</span>);
        parts.push(<span key={key++}>=</span>);
        parts.push(<span key={key++} className="code-string">{match[4]}</span>);
      }
      lastIndex = re.lastIndex;
    }
    if (lastIndex < line.length) parts.push(<span key={key++}>{line.slice(lastIndex)}</span>);
    return <div key={i}>{parts.length ? parts : line || '\u00A0'}</div>;
  });
}

Object.assign(window, { generateHTML, syntaxHighlight, parseRows, mdInline, mdParagraphs, escapeHtml });

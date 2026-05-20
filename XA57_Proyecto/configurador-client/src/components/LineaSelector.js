import React from 'react';

export default function LineaSelector({ lineas, selectedLinea, onSelect, error, t }) {
    return (
        <div style={{ marginBottom: 20 }}>
            <label style={{ 
                fontWeight: 600, fontSize: 12, display: "block", 
                marginBottom: 8, color: t.tx1, fontFamily: t.ffH 
            }}>
                Línea / Cromática <span style={{ color: '#ef4444' }}>* Obligatorio</span>
            </label>
            <div style={{ 
                display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(140px, 1fr))", 
                gap: "10px" 
            }}>
                {lineas.map((l) => (
                    <div 
                        key={l.id} 
                        onClick={() => onSelect(l)}
                        style={{
                            cursor: "pointer",
                            padding: "8px",
                            background: selectedLinea?.id === l.id ? t.bg4 : t.bg3,
                            border: selectedLinea?.id === l.id ? `1px solid ${t.sv1}` : `1px solid ${t.bd2}`,
                            borderRadius: 8,
                            display: "flex",
                            alignItems: "center",
                            gap: "10px",
                            transition: "all 0.2s"
                        }}
                    >
                        <div style={{ 
                            width: 30, height: 30, borderRadius: 4, 
                            background: `linear-gradient(135deg, ${l.colorPrimario} 50%, ${l.colorSecundario || l.colorPrimario} 50%)`,
                            border: `1px solid ${t.bd3}`
                        }} />
                        <div style={{ fontWeight: 600, fontSize: 12, color: selectedLinea?.id === l.id ? t.tx0 : t.tx1 }}>
                            {l.nombre}
                        </div>
                    </div>
                ))}
            </div>
            {error && <p style={{ color: '#ef4444', fontSize: 11, marginTop: 5 }}>{error}</p>}
        </div>
    );
}

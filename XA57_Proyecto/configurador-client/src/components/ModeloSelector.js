import React from 'react';

export default function ModeloSelector({ modelos, selectedId, onSelect, error, t }) {
    return (
        <div style={{ marginBottom: 20 }}>
            <label style={{ 
                fontWeight: 600, fontSize: 12, display: "block", 
                marginBottom: 8, color: t.tx1, fontFamily: t.ffH 
            }}>
                Modelo de Autobús <span style={{ color: '#ef4444' }}>* Obligatorio</span>
            </label>
            <div style={{ 
                display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(120px, 1fr))", 
                gap: "10px" 
            }}>
                {modelos.map((m) => (
                    <div 
                        key={m.id} 
                        onClick={() => onSelect(m.id)}
                        style={{
                            cursor: "pointer",
                            padding: "10px",
                            background: selectedId === m.id ? t.bg4 : t.bg3,
                            border: selectedId === m.id ? `1px solid ${t.sv1}` : `1px solid ${t.bd2}`,
                            borderRadius: 8,
                            textAlign: "center",
                            transition: "all 0.2s"
                        }}
                    >
                        <div style={{ fontWeight: 600, fontSize: 13, color: selectedId === m.id ? t.tx0 : t.tx1 }}>
                            {m.nombre}
                        </div>
                        <div style={{ fontSize: 10, color: t.tx3, marginTop: 2 }}>
                            {m.fabricante}
                        </div>
                    </div>
                ))}
            </div>
            {error && <p style={{ color: '#ef4444', fontSize: 11, marginTop: 5 }}>{error}</p>}
        </div>
    );
}

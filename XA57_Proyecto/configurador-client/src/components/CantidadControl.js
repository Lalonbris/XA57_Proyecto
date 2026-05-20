import React from 'react';

export default function CantidadControl({ cantidad, onChange, t }) {
    return (
        <div style={{ display: "flex", alignItems: "center", border: `1px solid ${t.bd2}`, borderRadius: 8, background: t.bg3 }}>
            <button 
                onClick={() => onChange(Math.max(1, cantidad - 1))} 
                style={{ width: 38, height: 44, background: "none", border: "none", color: t.tx2, cursor: "pointer" }}
            >
                &minus;
            </button>
            <span style={{ width: 30, textAlign: "center", fontSize: 14, fontWeight: 600, color: t.tx0 }}>
                {cantidad}
            </span>
            <button 
                onClick={() => onChange(cantidad + 1)} 
                style={{ width: 38, height: 44, background: "none", border: "none", color: t.tx2, cursor: "pointer" }}
            >
                +
            </button>
        </div>
    );
}

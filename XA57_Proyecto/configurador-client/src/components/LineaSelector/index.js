import React from 'react';
import './styles.css';

const LineaSelector = ({ lineas, selectedLinea, onSelect, error, t }) => {
    return (
        <div className="linea-selector-container">
            <label className="config-label" style={{ color: t.tx1 }}>* Línea / Cromática</label>
            <div className={`linea-grid ${error ? 'has-error' : ''}`} style={{ borderColor: error ? '#ef4444' : 'transparent' }}>
                {lineas.map((linea) => (
                    <div
                        key={linea.id}
                        className={`linea-card ${selectedLinea?.id === linea.id ? 'selected' : ''}`}
                        onClick={() => onSelect(linea)}
                        style={{
                            background: selectedLinea?.id === linea.id ? t.bg5 : t.bg3,
                            borderColor: selectedLinea?.id === linea.id ? t.bd3 : t.bd1,
                        }}
                    >
                        <div className="color-swatches">
                            <div className="swatch" style={{ backgroundColor: linea.colorPrimario, border: `2px solid ${t.bg1}` }}></div>
                            <div className="swatch" style={{ backgroundColor: linea.colorSecundario, border: `2px solid ${t.bg1}` }}></div>
                        </div>
                        <div className="linea-info">
                            <span className="linea-nombre" style={{ color: t.tx0 }}>{linea.nombre}</span>
                            <span className="linea-operador" style={{ color: t.tx2 }}>{linea.nombreOperador}</span>
                        </div>
                    </div>
                ))}
            </div>
            {error && <span className="error-message">{error}</span>}
        </div>
    );
};

export default LineaSelector;

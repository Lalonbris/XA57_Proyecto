import React from 'react';
import './styles.css';

const ModeloSelector = ({ modelos, selectedId, onSelect, error, t }) => {
    return (
        <div className="modelo-selector-container">
            <label className="config-label" style={{ color: t.tx1 }}>* Modelo de autobús</label>
            <div className={`modelo-grid ${error ? 'has-error' : ''}`} style={{ borderColor: error ? '#ef4444' : 'transparent' }}>
                {modelos.map((modelo) => (
                    <div
                        key={modelo.id}
                        className={`modelo-card ${selectedId === modelo.id ? 'selected' : ''}`}
                        onClick={() => onSelect(modelo.id)}
                        style={{
                            background: selectedId === modelo.id ? t.bg5 : t.bg3,
                            borderColor: selectedId === modelo.id ? t.bd3 : t.bd1,
                        }}
                    >
                        <span className="modelo-fabricante" style={{ color: t.tx2 }}>{modelo.fabricante}</span>
                        <span className="modelo-nombre" style={{ color: t.tx0 }}>{modelo.nombre}</span>
                    </div>
                ))}
            </div>
            {error && <span className="error-message">{error}</span>}
        </div>
    );
};

export default ModeloSelector;

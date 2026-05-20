import React from 'react';
import './styles.css';

const CharacterCount = ({ value, maxLength, t }) => {
    const length = value?.length || 0;
    const isError = length > maxLength;
    return (
        <span className={`char-count ${isError ? 'error' : ''}`} style={{ color: isError ? '#ef4444' : t.tx3 }}>
            {length}/{maxLength}
        </span>
    );
};

const PersonalizacionForm = ({ tipoProducto, valores, onChange, errores, t }) => {

    const handleUpdate = (e) => {
        onChange(e.target.name, e.target.value);
    };

    return (
        <div className="personalizacion-form-container">
            {tipoProducto?.permiteNombre && (
                <div className="form-group">
                    <label htmlFor="nombreOperador" style={{ color: t.tx1 }}>Nombre del operador (opcional)</label>
                    <input
                        type="text"
                        id="nombreOperador"
                        name="nombreOperador"
                        value={valores.nombreOperador || ''}
                        onChange={handleUpdate}
                        maxLength={tipoProducto.maxCaracteres}
                        style={{ background: t.bg1, borderColor: errores?.nombreOperador ? '#ef4444' : t.bd2, color: t.tx0 }}
                    />
                    <CharacterCount value={valores.nombreOperador} maxLength={tipoProducto.maxCaracteres} t={t} />
                    {errores?.nombreOperador && <span className="error-message">{errores.nombreOperador}</span>}
                </div>
            )}

            {tipoProducto?.permiteNumeroEconomico && (
                 <div className="form-group">
                    <label htmlFor="numeroEconomico" style={{ color: t.tx1 }}>Número económico (opcional)</label>
                    <input
                        type="text"
                        id="numeroEconomico"
                        name="numeroEconomico"
                        value={valores.numeroEconomico || ''}
                        onChange={handleUpdate}
                        style={{ background: t.bg1, borderColor: errores?.numeroEconomico ? '#ef4444' : t.bd2, color: t.tx0 }}
                    />
                    {errores?.numeroEconomico && <span className="error-message">{errores.numeroEconomico}</span>}
                </div>
            )}

            {tipoProducto?.permiteRuta && (
                <div className="form-group">
                    <label htmlFor="ruta" style={{ color: t.tx1 }}>Ruta (opcional)</label>
                    <input
                        type="text"
                        id="ruta"
                        name="ruta"
                        value={valores.ruta || ''}
                        onChange={handleUpdate}
                        style={{ background: t.bg1, borderColor: errores?.ruta ? '#ef4444' : t.bd2, color: t.tx0 }}
                    />
                    {errores?.ruta && <span className="error-message">{errores.ruta}</span>}
                </div>
            )}

            <div className="form-group">
                <label htmlFor="notasEspeciales" style={{ color: t.tx1 }}>Notas especiales (opcional)</label>
                <textarea
                    id="notasEspeciales"
                    name="notasEspeciales"
                    value={valores.notasEspeciales || ''}
                    onChange={handleUpdate}
                    rows="3"
                    style={{ background: t.bg1, borderColor: errores?.notasEspeciales ? '#ef4444' : t.bd2, color: t.tx0 }}
                ></textarea>
                {errores?.notasEspeciales && <span className="error-message">{errores.notasEspeciales}</span>}
            </div>
        </div>
    );
};

export default PersonalizacionForm;

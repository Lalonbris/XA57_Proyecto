import React from 'react';

const CantidadControl = ({ cantidad, setCantidad, t }) => {
    const handleIncrement = () => setCantidad(prev => prev + 1);
    const handleDecrement = () => setCantidad(prev => Math.max(1, prev - 1));
    const handleChange = (e) => {
        const value = parseInt(e.target.value, 10);
        if (!isNaN(value) && value >= 1) setCantidad(value);
        else if (e.target.value === '') setCantidad(1);
    };

    const wrapperStyle = {
        display: 'flex',
        alignItems: 'center',
        border: `1px solid ${t.bd2}`,
        borderRadius: '8px',
        width: '150px',
        overflow: 'hidden',
        background: t.bg1,
    };
    const btnStyle = {
        background: t.bg3, border: 'none', color: t.tx1,
        fontSize: '1.5rem', fontWeight: 'bold', cursor: 'pointer',
        width: '40px', height: '40px',
    };
    const inputStyle = {
        width: '70px', textAlign: 'center', border: 'none',
        background: 'transparent', color: t.tx0,
        fontSize: '1.2rem', fontWeight: 500,
    };

    return (
        <div>
            <label style={{ display: 'block', fontWeight: 500, marginBottom: '5px', fontSize: '0.85rem', color: t.tx1 }}>Cantidad</label>
            <div style={wrapperStyle}>
                <button onClick={handleDecrement} style={btnStyle}>-</button>
                <input type="number" value={cantidad} onChange={handleChange} style={inputStyle} min="1" />
                <button onClick={handleIncrement} style={btnStyle}>+</button>
            </div>
        </div>
    );
};

export default CantidadControl;

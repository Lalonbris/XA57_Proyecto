import React from 'react';

const VistaPrevia = ({ producto, modeloSeleccionado, lineaSeleccionada, personalizacion, t }) => {
    
    // This guard is essential. If the main product data isn't loaded, show a placeholder.
    if (!producto) {
        return <div style={{...containerStyle, background: t.bg1, textAlign: 'center', padding: '20px'}}>Cargando vista previa...</div>;
    }

    const imageUrl = producto.imagenUrl || 'https://via.placeholder.com/400x300.png?text=Imagen+no+disponible';

    return (
        <div style={containerStyle}>
            <div style={imageContainerStyle}>
                <img src={imageUrl} alt={producto.nombre} style={imageStyle} />
                <div style={overlayStyle(lineaSeleccionada)}></div>
                <div style={textoSuperpuesto}>
                    {/* THE FIX IS HERE: Use optional chaining on the 'personalizacion' prop directly */}
                    {personalizacion?.nombreOperador && <div style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>{personalizacion.nombreOperador}</div>}
                    {personalizacion?.numeroEconomico && <div style={{ fontSize: '2.5rem', fontWeight: 900 }}>{personalizacion.numeroEconomico}</div>}
                    {personalizacion?.ruta && <div style={{ fontSize: '1.2rem' }}>{personalizacion.ruta}</div>}
                </div>
            </div>
            <div style={{ padding: '20px' }}>
                <h4 style={{ marginTop: 0, fontSize: '1.3rem', color: t.tx0, fontFamily: t.ffH }}>{producto.nombre}</h4>
                <p style={{ margin: '5px 0', color: t.tx1 }}><strong>Modelo:</strong> {modeloSeleccionado?.nombre || 'No seleccionado'}</p>
                <p style={{ margin: '5px 0', color: t.tx1 }}><strong>Línea:</strong> {lineaSeleccionada?.nombre || 'No seleccionada'}</p>
                <div style={{ marginTop: '15px', fontSize: '1.2rem', fontWeight: 600, color: t.tx0 }}>
                    Precio: <span style={{ color: t.sv1, fontSize: '1.5rem' }}>${producto.precio?.toFixed(2) || '0.00'}</span>
                </div>
            </div>
        </div>
    );
};

// Styles are defined once outside the component for performance.
const containerStyle = {
    position: 'sticky', top: '20px',
    border: '1px solid #232323', borderRadius: '12px',
    overflow: 'hidden',
};
const imageContainerStyle = {
    position: 'relative', width: '100%',
    paddingTop: '75%', backgroundColor: '#070707',
};
const imageStyle = {
    position: 'absolute', top: 0, left: 0,
    width: '100%', height: '100%', objectFit: 'cover',
};
const textoSuperpuesto = {
    position: 'absolute', top: '50%', left: '50%',
    transform: 'translate(-50%, -50%)', textAlign: 'center',
    color: '#ffffff', textShadow: '2px 2px 4px rgba(0,0,0,0.7)',
};
const overlayStyle = (linea) => ({
    position: 'absolute', top: 0, left: 0,
    width: '100%', height: '100%',
    mixBlendMode: 'multiply', opacity: 0.8,
    background: linea ? `linear-gradient(45deg, ${linea.colorPrimario}B3, ${linea.colorSecundario}B3)` : 'transparent',
    transition: 'background 0.3s ease',
});

export default VistaPrevia;

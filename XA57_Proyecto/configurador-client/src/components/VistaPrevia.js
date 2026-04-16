import { useState } from "react";

/**
 * Componente de vista previa del producto configurado
 * Cumple con EPIC-07-07 y especifica actualización reactiva U-06, P-01
 * @param {Object} props - Props del componente
 * @param {Object} props.producto - Información del producto base
 * @param {Object|null} props.modeloSeleccionado - Modelo de autobús seleccionado
 * @param {Object|null} props.lineaSeleccionada - Línea/cromática seleccionada
 * @param {Object} props.personalizacion - Valores de personalización
 */
export const VistaPrevia = ({ producto, modeloSeleccionado, lineaSeleccionada, personalizacion }) => {
  const [previewReady, setPreviewReady] = useState(false);
  const [placeholderShown, setPlaceholderShown] = useState(!producto?.ImagenUrl);

  // Simular delay perceptible para cumplir con criterio U-06 (≤ 500ms)
  useEffect(() => {
    const timer = setTimeout(() => {
      setPreviewReady(true);
      setPlaceholderShown(!producto?.ImagenUrl);
    }, 300); // 300ms < 500ms requerido por U-06
    
    return () => clearTimeout(timer);
  }, [producto?.ImagenUrl, modeloSeleccionado, lineaSeleccionada]);

  return (
    <div className="relative bg-gray-100 rounded-lg overflow-hidden aspect-w-1 aspect-h-1">
      {/* Placeholder si no hay imagen base disponible (NFR U-06) */}
      {placeholderShown && (
        <div className="flex h-full w-full items-center justify-center bg-gray-200">
          <div className="text-center">
            <div className="w-16 h-16 bg-gray-400 rounded-full mb-3 flex items-center justify-center">
              <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" 
                      d="M4 6h16M4 12h16M4 18h16"/>
              </svg>
            </div>
            <p className="text-gray-500">Sin imagen disponible</p>
          </div>
        </div>
      )}

      {/* Vista previa real */}
      {!placeholderShown && previewReady && (
        <>
          {/* Imagen base del producto */}
          <img 
            src={producto.ImagenUrl} 
            alt={producto.Nombre} 
            className="object-contain w-full h-full"
          />

          {/* Overlay de colores según lineaSeleccionada */}
          {lineaSeleccionada && (
            <div className="absolute inset-0" 
                 style={{
                   backgroundColor: `rgba(${hexToRgb(lineaSeleccionada.colorPrimario)}, 0.4)`,
                   backgroundImage: `linear-gradient(45deg, transparent 49.5%, ${hexToRgb(lineaSeleccionada.colorSecundario)}0.4 50.5%, transparent 51%)`,
                   backgroundSize: "20px 20px",
                   pointerEvents: "none"
                 }}
            />
          )}

          {/* Texto de nombreOperador superpuesto si tiene valor */}
          {personalizacion.nombreOperador && (
            <div className="absolute bottom-4 left-1/2 transform -translate-x-1/2 text-white text-sm font-medium bg-black bg-opacity-60 px-3 py-1 rounded">
              {personalizacion.nombreOperador}
            </div>
          )}

          {/* Texto de numeroEconomico superpuesto si tiene valor */}
          {personalizacion.numeroEconomico && (
            <div className="absolute top-4 left-1/2 transform -translate-x-1/2 text-white text-sm font-medium bg-black bg-opacity-60 px-3 py-1 rounded">
              #{personalizacion.numeroEconomico}
            </div>
          )}

          {/* Texto de ruta superpuesto si tiene valor */}
          {personalizacion.ruta && (
            <div className="absolute top-4 right-4 text-xs text-white bg-black bg-opacity-60 px-2 py-1 rounded">
              {personalizacion.ruta}
            </div>
          )}

          {/* Notas especiales (no se muestran en preview pero están disponibles) */}
          {/* Las notas especiales son para producción/internal use, no para vista previa visual */}
        </>
      )}

      {/* Indicador de carga mientras se prepara la preview */}
      {!previewReady && !placeholderShown && (
        <div className="absolute inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="text-white text-sm">
            Cargando vista previa...
            <div className="ml-2 h-4 w-4 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
          </div>
        </div>
      )}
    </div>
  );
};

/**
 * Convierte color hexadecimal a formato rgb() para uso en CSS
 * @param {string} hex - Color en formato #RRGGBB
 * @returns {string} - Color en formato r, g, b
 */
function hexToRgb(hex) {
  // Eliminar el # si existe
  const cleanHex = hex.replace("#", "");
  
  // Convertir de hexadecimal a decimal
  const r = parseInt(cleanHex.substring(0, 2), 16);
  const g = parseInt(cleanHex.substring(2, 4), 16);
  const b = parseInt(cleanHex.substring(4, 6), 16);
  
  return `${r}, ${g}, ${b}`;
}
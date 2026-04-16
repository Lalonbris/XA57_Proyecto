import { useState } from "react";
import { validarConfiguracion } from "../utils/validaciones";

/**
 * Componente selector de líneas/cromáticas
 * Cumple con EPIC-07-05 y especifica validaciones U-02, RN-02
 * @param {Object} props - Props del componente
 * @param {Array} props.lineas - Lista de líneas disponibles
 * @param {number|null} props.selectedLinea - ID de la línea actualmente seleccionada
 * @param {Function} props.onSelect - Función callback cuando se selecciona una línea
 */
export const LineaSelector = ({ lineas, selectedLinea, onSelect }) => {
  const [error, setError] = useState("");

  const handleSelect = (lineaId) => {
    // Validar que se seleccionó una línea (RN-02)
    if (!lineaId) {
      setError("La línea / cromática es obligatoria");
      return;
    }
    
    setError(""); // Limpiar error si la selección es válida
    onSelect(lineaId);
  };

  // Determinar si hay error de validación (para mostrar visualmente)
  const tieneError = !selectedLinea && error;

  return (
    <div>
      <label className="block font-medium text-sm text-gray-700 mb-2">
        Línea / Cromática <span className="text-red-500">*</span>
      </label>
      
      <div className="grid grid-cols-2 gap-4 mt-1">
        {lineas.map(linea => (
          <div
            key={linea.id}
            onClick={() => handleSelect(linea.id)}
            className={`border-2 p-3 text-center cursor-pointer rounded-md 
                      ${linea.id === selectedLinea ? 
                        "border-primary-500 bg-primary-50" : 
                        "border-gray-300 hover:border-gray-400"}
                      ${tieneError && linea.id === selectedLinea ? "border-red-500" : ""}
                      ${error && linea.id === selectedLinea ? "bg-red-50" : ""}`}
          >
            <div className="flex items-center justify-center mb-2">
              <div className="w-10 h-10 rounded" 
                   style={{ 
                     background: `linear-gradient(135deg, ${linea.colorPrimario}, ${linea.colorSecundario})` 
                   }}>
              </div>
            </div>
            <p className="font-medium text-gray-800">{linea.nombre}</p>
            {linea.nombreOperador && (
              <p className="text-xs text-gray-500 mt-1">{linea.nombreOperador}</p>
            )}
          </div>
        ))}
      </div>
      
      {tieneError && (
        <p className="mt-2 text-sm text-red-600">{error}</p>
      )}
    </div>
  );
};
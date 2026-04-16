import { useState } from "react";
import { validarConfiguracion } from "../utils/validaciones";

/**
 * Componente selector de modelos de autobús
 * Cumple con EPIC-07-04 y especifica validaciones U-02, RN-02
 * @param {Object} props - Props del componente
 * @param {Array} props.modelos - Lista de modelos disponibles
 * @param {number|null} props.selectedId - ID del modelo actualmente seleccionado
 * @param {Function} props.onSelect - Función callback cuando se selecciona un modelo
 */
export const ModeloSelector = ({ modelos, selectedId, onSelect }) => {
  const [error, setError] = useState("");

  const handleSelect = (modeloId) => {
    // Validar que se seleccionó un modelo (RN-02)
    if (!modeloId) {
      setError("El modelo es obligatorio");
      return;
    }
    
    setError(""); // Limpiar error si la selección es válida
    onSelect(modeloId);
  };

  // Determinar si hay error de validación (para mostrar visualmente)
  const tieneError = !selectedId && error;

  return (
    <div>
      <label className="block font-medium text-sm text-gray-700 mb-2">
        Modelo de Autobús <span className="text-red-500">*</span>
      </label>
      
      <select
        value={selectedId || ""}
        onChange={(e) => handleSelect(Number(e.target.value))}
        className={`block w-full px-4 py-2 mt-1 border border-gray-300 rounded-md 
                  ${tieneError ? "border-red-500" : ""} 
                  ${error && "bg-red-50"}`}
        aria-invalid={tieneError ? "true" : "false"}
      >
        <option value="">Seleccione un modelo...</option>
        {modelos.map(modelo => (
          <option key={modelo.id} value={modelo.id}>
            {modelo.nombre} - {modelo.fabricante}
          </option>
        ))}
      </select>
      
      {tieneError && (
        <p className="mt-1 text-sm text-red-600">{error}</p>
      )}
    </div>
  );
};
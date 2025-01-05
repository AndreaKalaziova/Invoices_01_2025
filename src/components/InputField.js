import React from "react";

export function InputField(props) {
  // podporované typy pro element input
  const INPUTS = ["text", "number", "date"];

  // validace elementu a typu
  const type = props.type.toLowerCase();
  const isTextarea = type === "textarea";
  const required = props.required || false;

  if (!isTextarea && !INPUTS.includes(type)) {
    return null;
  }

  // přiřazení hodnoty minima a maxima do atributu příslušného typu
  const minProp = props.min || null;
  const maxProp = props.max || null;
  const min = ["number", "date"].includes(type) ? minProp : null;
  const max = ["number", "date"].includes(type) ? maxProp : null;
  const minlength = ["text", "textarea"].includes(type) ? minProp : null;
  const maxlength = ["text", "textarea"].includes(type) ? maxProp : null;

  return (
    <div className="form-group">
      <label>{props.label}:</label>

      {/* vykreslení aktuálního elementu */}
      {isTextarea ? (
        <textarea
          required={required}
          className="form-control"
          placeholder={props.prompt}
          rows={props.rows}
          minLength={minlength}
          maxLength={maxlength}
          name={props.name}
          value={props.value}
          onChange={props.handleChange}
          disabled={props.disabled} // to be able to disable the field
        />
      ) : (
        <input
          required={required}
          type={type}
          className={`form-control ${props.error ? "is-invalid" : ""}`} //for error messages to show
          placeholder={props.prompt}
          minLength={minlength}
          maxLength={maxlength}
          min={min}
          max={max}
          name={props.name}
          value={props.value}
          onChange={props.handleChange}
          disabled={props.disabled} // to be able to disable the field (identificationNumber in edit)
        />
      )}
      {props.error && <div className="invalid-feedback">{props.error}</div>}
    </div>
  );
}

export default InputField;

import React from "react";

export function InputSelect(props) {
  const multiple = props.multiple;
  const required = props.required || false;

  // příznak označení prázdné hodnoty
  const emptySelected = multiple ? props.value?.length === 0 : !props.value;
  // příznak objektové struktury položek
  const objectItems = props.enum ? false : true;

  return (
    <div className="form-group">
      <label>{props.label}:</label>
      <select
        required={required}
        className={`browser-default form-select ${props.error ? "is-invalid" : ""}`}
        //className="browser-default form-select"
        multiple={multiple}
        name={props.name}
        onChange={props.handleChange}
        value={props.value}
      >
        {required ? (
          //required ? true = insert& edit : false = filter

          props.allowEmptyInsert ? (
            //          allowEmpty ? true = insert : false = edit

            // Prázdná hodnota povolená pro vkládání záznamů 
            <option key={0} value={emptySelected}>
              {props.prompt}
            </option>

            // prázdná hodnota zakázaná (pro úpravu záznamu) 
          ) : (
            <option disabled value={emptySelected}>
              {props.prompt}
            </option>
          )
        ) : (
          // prázdná hodnota povolená (pro filtrování přehledu) 
          <option key={0} value={emptySelected}>
            ({props.prompt})
          </option>
        )}


        {objectItems
          ? /* vykreslení položek jako objektů z databáze (persons) */
          props.items.map((item, index) => (
            <option key={required ? index : index + 1} value={item._id}>
              {item.name}
            </option>
          ))
          : /* vykreslení položek jako hodnot z výčtu (invoices) */
          props.items.map((item, index) => (
            <option key={required ? index : index + 1} value={item}>
              {props.enum[item]}
            </option>
          ))}
      </select>
      {props.error && <div className="invalid-feedback">{props.error}</div>}
    </div>
  );
}

export default InputSelect;

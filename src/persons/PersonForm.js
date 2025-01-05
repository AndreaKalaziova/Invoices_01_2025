/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *                                _
 *              ___ ___ ___ _____|_|_ _ _____
 *             | . |  _| -_|     | | | |     |  LICENCE
 *             |  _|_| |___|_|_|_|_|___|_|_|_|
 *             |_|
 *
 *   PROGRAMOVÁNÍ  <>  DESIGN  <>  PRÁCE/PODNIKÁNÍ  <>  HW A SW
 *
 * Tento zdrojový kód je součástí výukových seriálů na
 * IT sociální síti WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci prémiového obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiGet, apiPost, apiPut } from "../utils/api";

import InputField from "../components/InputField";
import InputCheck from "../components/InputCheck";
import FlashMessage from "../components/FlashMessage";

import Country from "./Country";

//PersonForm for adding new person (if no 'id' provided) or eding person's details (if 'id' is provided) 
const PersonForm = () => {
    const navigate = useNavigate(); //hook for navigation, redirecting
    const { id } = useParams();       // extracting 'id' from URL to determine if editing or creating
    const [person, setPerson] = useState({
        name: "",
        identificationNumber: "",
        taxNumber: "",
        accountNumber: "",
        bankCode: "",
        iban: "",
        telephone: "",
        mail: "",
        street: "",
        zip: "",
        city: "",
        country: Country.CZECHIA,
        note: ""
    });                                             //initial state for person details

    const [isLoading, setIsLoading] = useState(true);
    const [sentState, setSent] = useState(false);   // tracks if the form submission was attempted
    const [successState, setSuccess] = useState(false); // racks if the submission was successful
    const [errorState, setError] = useState(null);      // tracks error messages
    const [validationErrors, setValidationErrors] = useState({}); // Track validation errors

    //fetch person's details if 'id' present - for edit
    useEffect(() => {
        if (id) {
            apiGet("/api/persons/" + id)
                .then((data) => {
                    setPerson(data);        // update person state with fetched data
                    setIsLoading(false);    // set loading to false after data is fetched
                })
                .catch((error) => {
                    console.error("Chyba při načítání detailu osoby:", error);
                    setFlashMessage("Chyba při načítání dat: " + error.message);
                    setFlashTheme("danger");
                    setIsLoading(false); // Stop loading even if there's an error
                });
        } else {
            setIsLoading(false);    // No id means this is a create operation; stop loading immediately
        }
    }, [id]);   //runs when 'id' changes

    const validateForm = () => {
        const errors = {};

        // Check required fields
        if (!person.name) errors.name = "Název je povinný.";
        if (!person.identificationNumber) errors.identificationNumber = "IČO je povinné.";
        if (!person.taxNumber) errors.taxNumber = "DIČ je povinné.";
        if (!person.accountNumber) errors.accountNumber = "Číslo účtu je povinné.";
        if (!person.bankCode) errors.bankCode = "Kód banky je povinný.";
        if (!person.iban) errors.iban = "IBAN je povinný.";
        if (!person.telephone) errors.telephone = "Tel.ˇčíslo je povinné.";
        if (!person.mail) errors.mail = "Email je povinný.";
        if (!person.street) errors.street = "Ulice je povinná.";
        if (!person.zip) errors.zip = "PSČ je povinné.";
        if (!person.city) errors.city = "Město je povinné.";
        if (!person.note) errors.note = "Poznámka je povinná.";

        // Validate telephone number using regex
        const telephoneRegex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{3}[-\s\.]?([0-9]{3,6})?$/;
        if (!person.telephone || !telephoneRegex.test(person.telephone)) {
            errors.telephone = "Invalidní formát telefonního čísla.";
        }
        // Validate email using regex
        const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
        if (!person.mail || !emailRegex.test(person.mail)) {
            errors.mail = "Invalidní formát emailu.";
        }
        //validate ientificationNumber length max 8 characters
        if (!person.identificationNumber || person.identificationNumber.length !== 8) {
            errors.identificationNumber = "IČO musí obsahovat přesně 8 znaků.";
        }

        setValidationErrors(errors);
        return Object.keys(errors).length === 0;

    };

    //handles the form submision
    //send POST for creation or PUT for edit 
    const handleSubmit = (e) => {
        e.preventDefault();

        if (!validateForm()) return;    // Stop submission if local validation fails.

        // if id present? true = PUT/edit : false = POST/create
        (id ? apiPut("/api/persons/" + id, person) : apiPost("/api/persons", person))
            .then((data) => {
                setSent(true);
                setSuccess(true);
                setTimeout(() => {
                    navigate("/persons");   //redirect to persons list after timeout
                }, 1800);
            })
            .catch((error) => {
                console.log(error.message);
                setError(error.message);
                setSent(true);
                setSuccess(false);
            });
    };

    const sent = sentState;         // track submision state
    const success = successState;   // track success state

    return (
        <div>
            <h1>{id ? "Upravit" : "Vytvořit"} osobnost</h1>
            <hr />
            {errorState ? (
                <div className="alert alert-danger">{errorState}</div>
            ) : null}
            {sent && (
                <FlashMessage
                    theme={success ? "success" : ""}
                    text={success ? "Uložení osobnosti proběhlo úspěšně." : "Nastala chyba při uložení"}
                />
            )}
            {/* Show loading message while fetching data */}
            {isLoading ? (
                <h2>Načítávám...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the PersonForm component is displayed
            ) : (
                <>
                    {/* Render form after data has been fetched */}
                    <form onSubmit={handleSubmit}>
                        <InputField
                            required={true}
                            type="text"
                            name="personName"
                            min="2"
                            max="100"
                            label="Jméno"
                            prompt="Zadejte celé jméno"
                            value={person.name}
                            handleChange={(e) => {
                                setPerson({ ...person, name: e.target.value });
                            }}
                            error={validationErrors.name}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="identificationNumber"
                            max="8"
                            label="IČO"
                            prompt="Zadejte IČO"
                            value={person.identificationNumber}
                            handleChange={(e) => {
                                setPerson({ ...person, identificationNumber: e.target.value });
                            }}
                            disabled={!!id} // if id present = update, the filed is disabled
                            error={validationErrors.identificationNumber}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="taxNumber"
                            min="8"
                            max="20"
                            label="DIČ"
                            prompt="Zadejte DIČ"
                            value={person.taxNumber}
                            handleChange={(e) => {
                                setPerson({ ...person, taxNumber: e.target.value });
                            }}
                            error={validationErrors.taxNumber}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="accountNumber"
                            min="8"
                            max="34"
                            label="Číslo bankovního účtu"
                            prompt="Zadejte číslo bankovního účtu"
                            value={person.accountNumber}
                            handleChange={(e) => {
                                setPerson({ ...person, accountNumber: e.target.value });
                            }}
                            error={validationErrors.accountNumber}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="bankCode"
                            min="3"
                            max="10"
                            label="Kód banky"
                            prompt="Zadejte kód banky"
                            value={person.bankCode}
                            handleChange={(e) => {
                                setPerson({ ...person, bankCode: e.target.value });
                            }}
                            error={validationErrors.bankCode}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="IBAN"
                            min="8"
                            max="34"
                            label="IBAN"
                            prompt="Zadejte IBAN"
                            value={person.iban}
                            handleChange={(e) => {
                                setPerson({ ...person, iban: e.target.value });
                            }}
                            error={validationErrors.iban}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="telephone"
                            label="Telefon"
                            prompt="Zadejte Telefon"
                            value={person.telephone}
                            handleChange={(e) => {
                                setPerson({ ...person, telephone: e.target.value });
                            }}
                            error={validationErrors.telephone}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="mail"
                            label="Mail"
                            prompt="Zadejte mail"
                            value={person.mail}
                            handleChange={(e) => {
                                setPerson({ ...person, mail: e.target.value });
                            }}
                            error={validationErrors.mail}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="street"
                            min="2"
                            max="50"
                            label="Ulice"
                            prompt="Zadejte ulici"
                            value={person.street}
                            handleChange={(e) => {
                                setPerson({ ...person, street: e.target.value });
                            }}
                            error={validationErrors.street}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="ZIP"
                            min="2"
                            max="10"
                            label="PSČ"
                            prompt="Zadejte PSČ"
                            value={person.zip}
                            handleChange={(e) => {
                                setPerson({ ...person, zip: e.target.value });
                            }}
                            error={validationErrors.zip}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="city"
                            min="2"
                            max="50"
                            label="Město"
                            prompt="Zadejte město"
                            value={person.city}
                            handleChange={(e) => {
                                setPerson({ ...person, city: e.target.value });
                            }}
                            error={validationErrors.city}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="note"
                            min="2"
                            max="150"
                            label="Poznámka"
                            value={person.note}
                            handleChange={(e) => {
                                setPerson({ ...person, note: e.target.value });
                            }}
                            error={validationErrors.note}
                        />

                        <h6>Země:</h6>
                        <InputCheck
                            type="radio"
                            name="country"
                            label="Česká republika"
                            value={Country.CZECHIA}
                            handleChange={(e) => {
                                setPerson({ ...person, country: e.target.value });
                            }}
                            checked={Country.CZECHIA === person.country}
                        />
                        <InputCheck
                            type="radio"
                            name="country"
                            label="Slovensko"
                            value={Country.SLOVAKIA}
                            handleChange={(e) => {
                                setPerson({ ...person, country: e.target.value });
                            }}
                            checked={Country.SLOVAKIA === person.country}
                        />

                        <input type="submit" className="btn btn-success" value="Uložit" />
                    </form>
                </>
            )}
        </div>
    );
};

export default PersonForm;

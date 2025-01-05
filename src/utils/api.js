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

// endpoints 
const API_URL = "";

const fetchData = (url, requestOptions) => {
    const apiUrl = `${API_URL}${url}`;

    return fetch(apiUrl, requestOptions)
        .then((response) => {
            if (!response.ok) {
                // If response is not ok, handle it as a special case
                if (response.status === 409) {
                    // If status is 409 (for checking duplicity of inv.number and ICO), parse and extract the error message from the body
                    return response.json().then((errorData) => {
                        throw { status: response.status, message: errorData.message || "Unexpected error" };
                    });
                }

                // Otherwise, throw a general error with status
                throw new Error(`Network response was not ok: ${response.status} ${response.statusText}`);
            }

            // If the method is not DELETE, return the response body as JSON
            if (requestOptions.method !== 'DELETE') {
                return response.json();
            }
        })
        .catch((error) => {
            // Catch any errors from above and propagate them
            throw error;
        });
};


export const apiGet = (url, params) => {
    const filteredParams = Object.fromEntries(
        Object.entries(params || {}).filter(([_, value]) => value != null)
    );

    const apiUrl = `${url}?${new URLSearchParams(filteredParams)}`;
    const requestOptions = {
        method: "GET",
    };

    return fetchData(apiUrl, requestOptions);
};

export const apiPost = async (url, data) => {
    const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    };

    try {
        const response = await fetchData(url, requestOptions);
        return response;  // If the response is successful, return it
    } catch (error) {
        // Handle 409 error (for checking duplici of inv.number and ICO)
        if (error.status === 409) {
            // Specific case for 409 error (duplicate invoice number and ICO)
            throw {
                status: error.status,
                message: error.message || "This number already exists",
            };
        }
        // Handle other errors (like network issues)
        throw error;
    }
};


export const apiPut = (url, data) => {
    const requestOptions = {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    };

    return fetchData(url, requestOptions);
};

export const apiDelete = (url) => {
    const requestOptions = {
        method: "DELETE",
    };

    return fetchData(url, requestOptions);
};

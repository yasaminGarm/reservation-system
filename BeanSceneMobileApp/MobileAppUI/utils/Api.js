import NetInfo from "@react-native-community/netinfo";
import cache from "./Caching";

// Root URL for the API backend
//const apiUrl = "https://localhost:7016/api";
const apiUrl = "https://localhost:7143/api";

// Use caching for semi-offline operation - set high TTL (time to live)
cache.ttlMinutes = 60;


/* * Create a GET request to a URL.
 * @param {string} url The request URL.
 * @param {object} data The data to pass through.
 * @param {bool} returnsData True if the response should return data.c
 * @returns {Promise} The response promise.
 */
async function getRequest(url, data = {}, returnsData = true) {
    
    // Build URL with data attached
    url += '?' + new URLSearchParams(data);
    
    // Make request, wait for response
    const response = await fetch(url, {
        method: 'GET',
        cache: 'no-cache', // Ignore caching
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
    })
    // Check for errors, e.g. 400, 500
    .then(handleFetchError)

    // Return response data if available
    return returnsData ? response.json() : Promise.resolve();
}


/**
 * Create a GET request to a URL while using the AsyncCache for offline data loading.
 * @param {string} url The request URL.
 * @param {object} data The data to pass through.
 * @param {bool} returnsData True if the response should return data.
 * @returns {Promise} The response promise.
 */
async function getRequestWithCaching(url, data = {}, returnsData = true) {
    
    // Use the original URL as the cache key
    const cacheKey = url

    // Get network state
    const networkState = await NetInfo.fetch()

    // Check if currently offline
    if (!networkState.isConnected) {

        // Load from cache if available (null if not)
        console.log(`OFFLINE: Load from cache: ${cacheKey}`)
        return Promise.resolve(await cache.getItem(cacheKey))
    }
    
    // Build URL with data attached
    url += '?' + new URLSearchParams(data);
    
    // Make request, wait for response
    const response = await fetch(url, {
        method: 'GET',
        cache: 'no-cache', // Ignore caching
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
    })
    // Check for errors, e.g. 400, 500
    .then(handleFetchError)

    // Update cache
    console.log(`Updating cache: ${cacheKey}`)
    cache.setItem(cacheKey, await response.clone().json())

    // Return response data if available
    return returnsData ? response.json() : Promise.resolve();
}


/**
 * Create a POST request to a URL.
 * @param {string} url The request URL.
 * @param {object} data The data to pass through.
 * @param {bool} returnsData True if the response should return data.
 * @returns {Promise} The response promise.
 */
async function postRequest(url, data = {}, returnsData = true) {
    


    // Make request, wait for response
    const response = await fetch(url, {
        method: 'POST',
        body: JSON.stringify(data),
        cache: 'no-cache', // Ignore caching
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
    })
    // Check for errors, e.g. 400, 500
    .then(handleFetchError);

    // Return response data if available
    return returnsData ? response.json() : Promise.resolve();
}


/**
 * Create a PUT request to a URL.
 * @param {string} url The request URL.
 * @param {object} data The data to pass through.
 * @param {bool} returnsData True if the response should return data.
 * @returns {Promise} The response promise.
 */
async function putRequest(url, data = {}, returnsData = false) {
    
    // Make request, wait for response
    const response = await fetch(url, {
        method: 'PUT',
        body: JSON.stringify(data),
        cache: 'no-cache', // Ignore caching
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
    })
    // Check for errors, e.g. 400, 500
    .then(handleFetchError);

    // Return response data if available
    return returnsData ? response.json() : Promise.resolve();
}


/**
 * Create a DELETE request to a URL.
 * @param {string} url The request URL.
 * @param {object} data The data to pass through.
 * @param {bool} returnsData True if the response should return data.
 * @returns {Promise} The response promise.
 */
async function deleteRequest(url, data = {}, returnsData = false) {
    
    // Make request, wait for response
    const response = await fetch(url, {
        method: 'DELETE',
        body: JSON.stringify(data),
        cache: 'no-cache', // Ignore caching
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8'
        },
    })
    // Check for errors, e.g. 400, 500
    .then(handleFetchError);

    // Return response data if available
    return returnsData ? response.json() : Promise.resolve();
}


/**
 * Check for 400-500 errors and custom messages from the server.
 * @param {Response} response The Fetch API Response object.
 * @returns {Response} The original Response object.
 */
async function handleFetchError(response) {

    // Check for errors, e.g. 400, 500
    if (!response.ok) {

        // Check for custom error message from API
        if (response.message) {
            console.log(response.message)
            throw Error(`API ${response.status} error: ${response.message}`);
        } else {
            console.log(response.statusText)
            throw Error(`API ${response.status} error: ${response.statusText}`);
        }
    }

    return response;
}


/*
 * ADD YOUR API CALLING METHODS HERE
 */

//Get Product
export function GetProductById(id) {

    // Call API endpoint: GET /Product
    return getRequestWithCaching(`${apiUrl}/product/${id}`)
        .then(response => {
            // If request/response is successful, return JSON data
            return response
        })

}


// Get all Products
export function GetProducts() {

    // Call API endpoint: GET /products
    return getRequestWithCaching(`${apiUrl}/Product`)
        .then(response => {
            // If request/response is successful, return JSON data
            return response
        })

}


export function GetTables() {

    // Call API endpoint: GET /Tables
    return getRequestWithCaching(`${apiUrl}/tables`)
        .then(response => {
            // If request/response is successful, return JSON data
            return response
        })

}

export function Login(user) {

    // Call API endpoint: POST /Login
    return postRequest(`${apiUrl}/User/login/${user.username}`, user)
        .then(response => {
            // If request/response is successful, return JSON data
            return response
        })

}


export function AddToOrderApi(tableNumber, menuItem) {

    // Call API endpoint: POST /Add to Order
    return postRequest(`${apiUrl}/Order/${tableNumber}`, menuItem)
        .then(response => {
            // If request/response is successful, return JSON data
            return response;
        })

}

export function GetOrders(tableNumber) {
        // Call API endpoint: GET /Orders
        return getRequestWithCaching(`${apiUrl}/Order/${tableNumber}`)
        .then(response => {
            // If request/response is successful, return JSON data
            return response
        })
}


export function EditOrder(tableId,orderItems) {

    // Call API endpoint: POST /Order
    return putRequest(`${apiUrl}/Order/${tableId}`, orderItems)
        .then(response => {
            // If request/response is successful, return JSON data
            return true
        })

}



export function DeleteOrderItem(tableId,orderItemId) {
    

    // Call API endpoint: Delete /Order
    return deleteRequest(`${apiUrl}/Order/orderItem/${tableId}/${orderItemId}`)
        .then(response => {
            // If request/response is successful, return JSON data
            return true
        })

}

export function GetMatchingReservationByTableId(tableId) {
    // Call API endpoint: GET /Matching reservation
    return getRequestWithCaching(`${apiUrl}/order/matchingReservation/${tableId}`)
    .then(response => {
        // If request/response is successful, return JSON data
        return response
    })
}


//staus seated when taking order
export function updateReservationStatus(reservationId, status) {

    // Call API endpoint: POST /Update Reservation
    return putRequest(`${apiUrl}/order/updateReservationStatus/${reservationId}/${status}`)
        .then(response => {
            // If request/response is successful, return JSON data
            return true
        })

}
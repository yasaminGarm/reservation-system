import { deleteToken, getToken, storeToken } from "./TokenStorage";

//const BASE_URL = 'http://10.0.2.2:5136';
//const BASE_URL = 'http://10.0.2.2:5171';
const BASE_URL = 'http://localhost:5132/';

//const BASE_URL = 'http://localhost:7143';

export async function login(username, password) {
    const url = new URL('api/v1/token', BASE_URL);

    try{
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email: username,
            password: password
        })
        
    });
    if (!response.ok) {
        // Handle HTTP errors
        throw new Error(`HTTP error: ${response.status} ${response.statusText}`);
    }

    // May need to change this at some point
    const token = await response.text();

    await storeToken(token);

    return {
        authenticated: true,
        token: token
    };
}
    catch(error){// Handle network errors
        console.error('Network error:', error);
        throw error; // Propagate the error to the caller}
}
}
export async function signOut() {
    deleteToken();
}

export async function fetchSecuredResource() {
    const url = new URL('api/v1/test/authorize', BASE_URL);
    const token = await getToken();

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (response.status === 401) {
        throw new Error('You must be authenticated to access this resource!');
    } else if (response.status === 403) {
        throw new Error('You do not have the authorization to access this resource!');
    }

    const data = await response.text();
    
    return { data: data };
}

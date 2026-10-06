import type{ LoginResponse, LoginRequest } from '../types/auth';

export async function login(loginRequest: LoginRequest): Promise<LoginResponse> {
    const response = await fetch(`https://localhost:7276/api/Auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginRequest)
    });

    if(!response.ok) {
        throw new Error(`Login Failed : ${response.status}`);
    }

    const loginResponse: LoginResponse = await response.json();

    return loginResponse;
}
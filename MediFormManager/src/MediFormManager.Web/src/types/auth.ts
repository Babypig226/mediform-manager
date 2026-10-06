export interface LoginRequest {
    loginId: string;
    password: string;
}

export interface LoginResponse {
    accessToken: string;
    expiresAt: string;
    userId: string;
    loginId: string;
    userName: string;
    roleName: string;
}
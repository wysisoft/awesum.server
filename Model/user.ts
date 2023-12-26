export interface User {
    id: number;
    email: string;
    loginid: string | null;
    name: string | null;
    apps: App[];
}
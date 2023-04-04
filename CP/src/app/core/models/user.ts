import { UserRole } from "./user-role";

export class User {

    public id?: string;
    public username: string;
    public email: string;
    public phoneNumber: string;
    public fullName: string;
    public profileImage?: string;
    public isOnline: boolean;
    public isActive: boolean;
    public emailConfirmed: boolean;
    public roles?: UserRole[];
    public claims?: string[];
    public password: string;
    public profileImageFile: File;
}
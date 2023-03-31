export class User {

    public id?: string;
    public username: string;
    public email: string;
    public phoneNumber: string;
    public fullName: string;
    public profileImage?: string;
    public isOnline: boolean;
    public isActive: boolean;
    public roles?: string[];
    public claims?: string[];
    public profileImageFile: File;
}
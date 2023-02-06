import { JwtToken } from "./jwt-token";
import { User } from "./user";

export class AuthResult {
    public jwt: JwtToken;
    public user: User;
}
import { JwtToken } from "./jwt-token";
import { User } from "../users/user";

export class AuthResult {
    public jwt: JwtToken;
    public user: User;
}
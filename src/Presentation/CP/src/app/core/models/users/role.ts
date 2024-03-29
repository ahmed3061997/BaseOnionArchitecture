import { CultureLookup } from "../common/culture-lookup";

export class Role {
    public id?: string;
    public name?: string;
    public isActive: boolean;
    public claims?: string[];
    public names?: CultureLookup[];
}
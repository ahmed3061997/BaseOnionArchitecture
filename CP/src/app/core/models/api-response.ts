export class ApiResponse<T = any> {
    public result: boolean;
    public errors: string[];
    public value: T;
}
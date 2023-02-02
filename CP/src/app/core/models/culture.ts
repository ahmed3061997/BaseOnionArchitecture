export class Culture {
    public code: string = '';
    public name: string = '';
    public flag: string = '';
    public isDefault: boolean = false;
    
    constructor(code: string = '', name: string= '', flag: string= '', isDefault: boolean = false) {
        this.code = code;
        this.name = name;
        this.flag = flag;
        this.isDefault = isDefault;
    }
}
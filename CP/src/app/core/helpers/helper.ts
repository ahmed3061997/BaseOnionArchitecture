export class Helper {

    public static groupBy(arr: any[], key: any) : any {
        return arr.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    };
}
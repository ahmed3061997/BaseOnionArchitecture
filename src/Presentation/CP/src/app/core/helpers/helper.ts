export class Helper {

    public static groupBy(arr: any[], key: any) : any {
        return arr.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    };

    public static saveAsFile(fileName: string, content: any, type: string = 'application/json') {
        // Create a blob from the file content
        const blob = new Blob([content], { type });
    
        // Create a link to the blob
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
    }
}
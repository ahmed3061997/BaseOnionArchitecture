export function AutoUnsubscribe(): ClassDecorator {
    return function (constructor) {
        const orig = constructor.prototype.ngOnDestroy
        constructor.prototype.ngOnDestroy = function () {
            for (const prop in this) {
                const property = this[prop]
                if (typeof property.unsubscribe === "function") {
                    property.unsubscribe()
                }
            }
            orig?.apply()
        }
    }
}
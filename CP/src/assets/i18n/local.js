import LocalizedStrings from 'react-localization';

let context = require.context('../lang/', false, /\.json$/);
let files = {};

context.keys().forEach((filename) => {
    if (filename.endsWith(".json")) {
        const fileContext = context(filename);
        Object.keys(fileContext).forEach(local => {
            var source = fileContext[local];
            var target = files[local];
            if (target == null) {
                files[local] = {};
                target = files[local];
            }
            Object.keys(source).forEach(key => target[key] = source[key]);
        })
    }
});

var locals = ['en', 'ar'];
function local() {
    var defaultLocal = locals[0];
    if (arguments.length == 1) {
        var lang = arguments[0] || defaultLocal;
        lang = locals.indexOf(lang) == -1 ? defaultLocal : lang;
        localStorage.setItem('local', lang);
    } else {
        return localStorage.getItem('local') || defaultLocal;
    }
}

function isRtl() {
    return local() == 'ar';
}

const Locale = new LocalizedStrings(files);
export { Locale, local, isRtl };
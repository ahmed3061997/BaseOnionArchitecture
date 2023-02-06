$('.dropdown')
    .each((i, e) => {
        var speed = $(e).data('animation-speed'); // slow, slower, fast, faster
        var showAnim = ($(e).data('open-animation') ? `animate__${$(e).data('open-animation')}` : null) || 'animate__fadeIn';
        var hideAnim = ($(e).data('close-animation') ? `animate__${$(e).data('close-animation')}` : null) || 'animate__fadeOut';

        var menu = $(e).find('.dropdown-menu');
        if (speed) menu.addClass('animate__' + speed);
        menu.addClass('animate__animated').addClass(showAnim);
        $(e).find('.dropdown-toggle').on('click', function () {

            menu.on('animationend', () => {
                menu.removeClass(showAnim);
                menu.off('animationend');
            }).addClass(showAnim);

            $(e).on('hide.bs.dropdown', function (e) {
                e.preventDefault();
                menu.on('animationend', () => {
                    menu.removeClass(hideAnim);
                    menu.removeClass('show');
                    menu.off('animationend');
                }).addClass(hideAnim);
                $(e).off('hide.bs.dropdown');
            })
        })
    });
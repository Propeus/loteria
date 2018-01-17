$(".btnGerar").click(function () {
    var form = $("#" + $(this).attr('form'));
    var url = $(form).attr("action");
    var method = $(form).attr("method");
    $.ajax({
        url: url,
        method: method,
        success: function (e) {
            var numerosSoretiosInput = $(".NumeroSorteio");
            for (var i = 0; i < numerosSoretiosInput.length; i++) {
                $(numerosSoretiosInput[i]).val(e.Numeros[i]);
            }
            $(".SorteioAposta").val(e.Numeros.join(''));
            $(".SorteioApostaExibicao").val(e.Numeros.join('-'));
        }
    })
})


$('.NumeroSorteio').change(function () {
    var numerosSoretiosInput = $(".NumeroSorteio");
    var numeros = [];
    for (var i = 0; i < numerosSoretiosInput.length; i++) {
        numeros[i] = $(numerosSoretiosInput[i]).val();
    }
    $(".SorteioAposta").val(numeros.join(''));
    $(".SorteioApostaExibicao").val(numeros.join('-'));
})
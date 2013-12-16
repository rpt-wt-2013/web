function setInputValue(input_id, val) {
    document.getElementById(input_id).setAttribute('value', val);
}

function showElement(element_id) {
    $("#" + element_id).show(100);
}

function hideElement(element_id) {
    $("#" + element_id).hide(100);
}

function switchShowElement(element_id) {
    var element = $("#" + element_id);
    if ($("#" + element_id).getAttribute("display") == "none") {
        showElement(element_id);
    } else {
        hideElement(element_id);
    }
    showElement(element_id);
}

function setFormularValue(element, value_type, input_id ,form_id) {
    var children = document.getElementById(form_id).getElementsByTagName("input");
    for (var i = 0; i < children.length; i++) {
        if (children[i].getAttribute('id') == input_id) {
            children[i].setAttribute('value', element.getAttribute(value_type));
        }
    }
}

function showFormular(element, form_id) {
    var children = document.getElementById(form_id).getElementsByTagName("input");
    for (var i = 0; i < children.length; i++) {
        if (children[i].getAttribute('id') == 'File') {
            children[i].setAttribute('value',element.getAttribute("data-file"));
        }
        if (children[i].getAttribute('type') == 'Submit') {
            children[i].onclick = hideElement(form_id);
        }
    }
    showElement(form_id);
}
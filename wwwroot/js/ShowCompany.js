document.getElementById("markdown-text").innerHTML = markdown.toHTML(text);
let markChecks = document.querySelectorAll(".mark-check");
let newsText = document.querySelectorAll(".news-text");
for (let i = 0; i < newsText.length; i++) {
    let buf = newsText[i].textContent;
    newsText[i].textContent = "";
    newsText[i].innerHTML = markdown.toHTML(buf);
}
let starChecks = document.querySelectorAll(".star-img");
let info = document.querySelectorAll(".main-section");
let select = 0;

document.getElementById("info").addEventListener("click", function () { SwitchSection(0); });
document.getElementById("news").addEventListener("click", function () { SwitchSection(1); });
document.getElementById("bonuses").addEventListener("click", function () { SwitchSection(2); });
document.getElementById("comments").addEventListener("click", function () { SwitchSection(3); });

function SwitchSection(num) {
    if (info[num].classList.contains("d-none")) {
        info[num].classList.remove("d-none");
        if (select != null)
            info[select].classList.add("d-none");
        select = num;
    } else {
        info[num].classList.add("d-none");
        select = null;
    }
}

for (let i = 0; i < markChecks.length; i++) {
    markChecks[i].addEventListener("click", function (e) {
        let buf = e.target.checked;
        for (let i = 0; i < markChecks.length; i++) {
            markChecks[i].checked = false;
            starChecks[i].src = "/img/star_f.png";
        }
        if (buf) {
            e.target.checked = true;
            for (let i = 0; i < markChecks.length; i++) {
                starChecks[i].src = "/img/star_t.png";
                if (markChecks[i].checked)
                    break;
                markChecks[i].checked = true;
            }
        }
    });
}

document.ondragover = function (e) { e.preventDefault() }
document.ondrop = function (e) { e.preventDefault(); upload(e.dataTransfer.files[0]); }
function upload(file) {

    if (!file || !file.type.match(/image.*/)) return;

    let spanUploading = document.createElement('div');
    spanUploading.classList.add("fixed-bottom");
    spanUploading.innerHTML = "Uploading";
    document.body.appendChild(spanUploading);

    var fd = new FormData();
    fd.append("image", file);
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "https://api.imgur.com/3/image.json");
    xhr.onload = function () {
        let linkImage = JSON.parse(xhr.responseText).data.link;
        document.querySelector('.company-image').src = linkImage;

        document.getElementById("company-image").setAttribute('value', linkImage);

        spanUploading.innerHTML = "Uploaded";
    }

    xhr.setRequestHeader('Authorization', 'Client-ID 28aaa2e823b03b1');

    xhr.send(fd);
}
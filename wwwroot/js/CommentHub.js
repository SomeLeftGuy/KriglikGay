let hubUrl = 'https://localhost:44333/Comments';
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .configureLogging(signalR.LogLevel.Information)
    .build();
hubConnection.on('Send', function (message, userName, comment) {
    let messages = document.querySelectorAll(".author");
    let dublicate = false;
    for (let i = 0; i < messages.length; i++) {
        if (messages[i].textContent == userName) {
            dublicate = true;
            if (message == "")
                messages[i].parentNode.remove();
            else
                messages[i].parentNode.querySelector(".text-context").textContent = message;
        }
    }
    if (!dublicate) {
        let container = document.createElement("div");
        container.classList.add("border");
        container.classList.add("rounded");
        container.classList.add("p-3");
        container.classList.add("mb-3");

        let userNameElem = document.createElement("h2");
        userNameElem.appendChild(document.createTextNode(userName));
        userNameElem.classList.add("author");

        let elem = document.createElement("p");
        elem.classList.add("text-context")
        elem.appendChild(document.createTextNode(message));
        let ratingSpan = document.createElement("span");
        ratingSpan.appendChild(document.createTextNode("Rating: "));
        let rating = document.createElement("span");
        rating.classList.add("rating");
        rating.appendChild(document.createTextNode("0"));
        let like = document.createElement("a");
        like.classList.add("like");
        let likeImg = document.createElement("img");
        likeImg.src = "/img/like.png";
        likeImg.width = "18";
        like.appendChild(likeImg);
        like.classList.add("ml-3");
        let dislike = document.createElement("a");
        dislike.classList.add("dislike");
        let dislikeImg = document.createElement("img");
        dislikeImg.src = "/img/dislike.png";
        dislikeImg.width = "18";
        dislike.appendChild(dislikeImg);
        dislike.classList.add("ml-3");
        let commentId = document.createElement("input");
        commentId.classList.add("comment-id");
        commentId.type = "hidden";
        commentId.value = comment;

        container.appendChild(userNameElem);
        container.appendChild(elem);
        container.appendChild(ratingSpan);
        container.appendChild(rating);
        container.appendChild(like);
        container.appendChild(dislike);
        container.appendChild(commentId);
        like.addEventListener("click", likeListener);
        dislike.addEventListener("click", dislikeListener);

        var firstElem = document.getElementById("comments-list").firstChild;
        document.getElementById("comments-list").insertBefore(container, firstElem);
    }

});
hubConnection.on('Rating', function (commentId, changeValue) {
    let comments = document.querySelectorAll(".comment-id");
    for (let i = 0; i < comments.length; i++) {
        if (comments[i].value == commentId)
            comments[i].parentNode.querySelector(".rating").textContent = parseInt(comments[i].parentNode.querySelector(".rating").textContent) + changeValue;
    }
});
document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("comment").value;
    hubConnection.invoke('Send', message, userName, company);
});
document.addEventListener("keydown", function (event) {
    if (event.code == "Enter") {
        let message = document.getElementById("comment").value;
        hubConnection.invoke('Send', message, userName, company);
    }
});
let likes = document.querySelectorAll(".like");
let dislikes = document.querySelectorAll(".dislike");
let likeListener = function (e) {
    let comment = e.currentTarget.parentNode.querySelector(".comment-id").value;
    hubConnection.invoke('Rating', userName, true, comment);
}
let dislikeListener = function (e) {
    let comment = e.currentTarget.parentNode.querySelector(".comment-id").value;
    hubConnection.invoke('Rating', userName, false, comment);
}
for (let i = 0; i < likes.length; i++) {
    likes[i].addEventListener("click", likeListener);
    dislikes[i].addEventListener("click", dislikeListener);
}

hubConnection.start();
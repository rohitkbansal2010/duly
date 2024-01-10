'use strict';

export class YouTubeApi {
    constructor() {
        this.connectYouTubeApi();
    }

    grabYouTubeId(url) {
        let regex = url.split(/(vi\/|v=|\/v\/|youtu\.be\/|\/embed\/)/);
        return (regex[2] !== undefined) ? regex[2].split(/[^0-9a-z_\-]/i)[0] : regex[0];
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click" || event === "touchmove") {
            return true;
        } else {
            return false;
        }
    }

    getVideoJson(id, element) {
        let url = '/services/youtube-api?id=' + id;

        fetch(url, {
            method: 'GET',
            credentials: 'same-origin',
            async: true,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }).then(async (response) => {
            const json = await response.json();
            return response.ok ? json : Promise.reject(json);
        }).then((response) => {
            let thumbnail = response['thumbnail_url'];
            let title = response['title'];

            let imgContainer = element.querySelector('.image');
            let img = document.createElement('img');
            img.src = thumbnail;
            img.alt = title;
            imgContainer.appendChild(img);

            element.querySelector('span').innerHTML = "https://www.youtube.com/embed/" + id + "?enablejsapi=1";

            let header = element.querySelector('.title');
            header.innerHTML = title;
        }).catch((response) => {
            console.log("YouTube API Error:" + response);
        });
    }

    replaceMainVideo(element) {
        let main = document.querySelector('#hero-video');
        main.querySelector('iframe').src = element.querySelector('.hidden').innerHTML;

        let mainVideo = main.querySelector('.video');
        let mainContent = mainVideo.querySelector('.content');
        let elementContent = element.querySelector('.content');

        let cloneElement = elementContent.cloneNode(true);
        let cloneMain = mainContent.cloneNode(true);

        mainVideo.appendChild(cloneElement);
        element.appendChild(cloneMain);

        element.removeChild(elementContent);
        mainVideo.removeChild(mainContent);
    }

    addEventListeners(videos) {
        let clickEvents = ['click', 'keydown'];

        clickEvents.forEach((event) => {
            videos.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        this.replaceMainVideo(element);
                    }
                });
            })
        });
    }
    
    pauseVideo() {
        if (document.querySelector('#iframe-video')) {
            // https://stackoverflow.com/a/8668741
            let iframe = document.querySelector('#iframe-video').contentWindow;
            iframe.postMessage('{"event":"command","func":"pauseVideo","args":""}', '*');
        }
    }

    connectYouTubeApi() {
        const container = document.querySelector('.service-videos-section');
        const videos = container.querySelectorAll('.video');
        const mainVideoSrc = container.querySelector('iframe').src;
        const mainId = this.grabYouTubeId(mainVideoSrc);
        container.querySelector('iframe').src = "https://www.youtube.com/embed/" + mainId + "?enablejsapi=1";

        for (let i = 0; i < videos.length; i++) {
            let video = videos[i];
            let link = video.querySelector('span').innerHTML;
            let id = this.grabYouTubeId(link);

            if (id) {
                this.getVideoJson(id, video);
            }
        }

        this.addEventListeners(videos);
    }
}
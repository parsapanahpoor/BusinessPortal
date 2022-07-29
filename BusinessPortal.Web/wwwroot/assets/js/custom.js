var swiper = new Swiper(".mySwiper", {
  speed: 800,
  loop: true,
  breakpoints: {
    576: {
      slidesPerView: 1,
    },
    768: {
      slidesPerView: 2,
    },
    1200: {
      slidesPerView: 5,
    },
  },
  autoplay: {
    delay: 2000,
    reverseDirection: true,
    stopOnLastSlide: false,
  },
  // If we need pagination
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
});

let searchDesktop = document.querySelector("#nav-item-search-desktop")

 let searchInput = document.querySelector(".search-input")
let searchBox = document.querySelector(".search-box-js")
let isOpenSearchBar = false
searchDesktop.addEventListener("click", function () {
    //searchBox.classList.toggle("active-box")
    //searchBox.style.display = "flex"
    if (isOpenSearchBar === false) {
        isOpenSearchBar = true
        searchBox.style.display = "flex"
    } else {
        isOpenSearchBar = false
        searchBox.style.display = 'none'
    }

    searchInput.focus();
})

//console.log(searchBox)

let megaMenuBtn = document.querySelector(".mega-menu-btn");
let megaMenuList = document.querySelector(".mega-menu-list");
megaMenuBtn.addEventListener("click", function () {
    megaMenuList.classList.toggle("mega-menu-list-active");
});


let megaMenuBtnServices = document.querySelector(".mega-menu-btn-Services");
let megaMenuListServices = document.querySelector(".mega-menu-list-Services");
megaMenuBtnServices.addEventListener("click", function () {
    megaMenuListServices.classList.toggle("mega-menu-list-active-Services");
});
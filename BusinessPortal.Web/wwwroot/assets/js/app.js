let latsetBuy = document.querySelector(".latset-buy-list");
let latsetProducts = document.querySelector(".latset-products-list");

latsetBuy.scrollTop = 1;
latsetProducts.scrollTop = 1;

let autoScrollBottomBuy = setInterval(() => {
  latsetBuy.scrollTop++;
}, 80);

let autoScrollBottomProducts = setInterval(() => {
  latsetProducts.scrollTop++;
}, 80);

let autoScrollTopBuy = setInterval(() => {
  if (latsetBuy.offsetHeight + latsetBuy.scrollTop >= latsetBuy.scrollHeight) {
    setInterval(() => {
      latsetBuy.scrollTop--;
    }, 500);
    clearInterval(autoScrollBottomBuy);
  }
}, 80);

let autoScrollTopProducts = setInterval(() => {
  if (
    latsetProducts.offsetHeight + latsetProducts.scrollTop >=
    latsetProducts.scrollHeight
  ) {
    setInterval(() => {
      latsetProducts.scrollTop--;
    }, 500);
    clearInterval(autoScrollBottomProducts);
  }
}, 80);

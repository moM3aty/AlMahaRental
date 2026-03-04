document.addEventListener('DOMContentLoaded', function () {
  const langToggle = document.getElementById("langToggle");
  const langFlag = document.getElementById("langFlag");
  let currentLang = localStorage.getItem("siteLang") || "ar";

  let headerSwiper = null;
  let fleetSwiper = null;

  function initSwipers() {
    if (typeof Swiper === "undefined") return;

    if (headerSwiper?.destroy) headerSwiper.destroy(true, true);
    if (fleetSwiper?.destroy) fleetSwiper.destroy(true, true);

    const isRTL = document.documentElement.dir === 'rtl';

    if (document.querySelector(".headerSwiper")) {
      headerSwiper = new Swiper(".headerSwiper", {
        loop: true,
        effect: "fade",
        speed: 1000,
        rtl: isRTL,
        autoplay: { delay: 4000, disableOnInteraction: false },
      });
    }

    if (document.querySelector(".fleetSwiper")) {
      fleetSwiper = new Swiper(".fleetSwiper", {
        slidesPerView: 3,
        spaceBetween: 30,
        loop: true,
        rtl: isRTL,
        grabCursor: true,
        autoplay: { delay: 3000, disableOnInteraction: false },
        speed: 800,
        breakpoints: { 0: { slidesPerView: 1 }, 768: { slidesPerView: 2 }, 1200: { slidesPerView: 3 } },
      });
    }
  }

  function switchLanguage(lang) {
    document.querySelectorAll("[data-ar]").forEach(el => {
      if (el.tagName === "INPUT" || el.tagName === "TEXTAREA") {
        el.placeholder = el.getAttribute(`data-${lang}`);
      } else if (el.tagName === "SELECT") {
        el.querySelectorAll("option").forEach(option => {
          const dataAttr = option.getAttribute(`data-${lang}`);
          if (dataAttr) option.textContent = dataAttr;
        });
      } else {
        el.textContent = el.getAttribute(`data-${lang}`);
      }
    });

    document.querySelectorAll("button[data-ar]").forEach(btn => {
      btn.textContent = btn.getAttribute(`data-${lang}`);
    });

    document.documentElement.lang = lang;
    if (lang === "ar") {
      document.documentElement.dir = "rtl";
      langFlag.src = "https://flagcdn.com/w40/us.png";
      langFlag.alt = "USA Flag";
    } else {
      document.documentElement.dir = "ltr";
      langFlag.src = "https://flagcdn.com/w40/sa.png";
      langFlag.alt = "Saudi Arabia Flag";
    }

    currentLang = lang;
    localStorage.setItem("siteLang", lang);

    setTimeout(initSwipers, 100);
  }

  langToggle?.addEventListener("click", () => {
    switchLanguage(currentLang === "ar" ? "en" : "ar");
  });

  const menuToggle = document.getElementById('menuToggle');
  const sidebarMenu = document.getElementById('sidebarMenu');
  const sidebarClose = document.getElementById('sidebarClose');
  const sidebarOverlay = document.getElementById('sidebarOverlay');

  function openSidebar() {
    sidebarMenu?.classList.add('active');
    sidebarOverlay?.classList.add('active');
    document.body.classList.add('sidebar-open');
  }
  function closeSidebar() {
    sidebarMenu?.classList.remove('active');
    sidebarOverlay?.classList.remove('active');
    document.body.classList.remove('sidebar-open');
  }

  menuToggle?.addEventListener('click', openSidebar);
  sidebarClose?.addEventListener('click', closeSidebar);
  sidebarOverlay?.addEventListener('click', closeSidebar);
  document.addEventListener('keydown', e => { if (e.key === 'Escape') closeSidebar(); });

  const tabButtons = document.querySelectorAll('.tab-btn');
  const dailyReserve = document.querySelector('.daily-reserve');
  const monthReserve = document.querySelector('.month-reserve');

  function safeDisplay(el, value) { if (el) el.style.display = value; }
  function updateFormFields(tab) {
    if (tab === 'daily' || tab === 'weekly') {
      safeDisplay(dailyReserve, 'flex');
      safeDisplay(monthReserve, 'none');
    } else if (tab === 'monthly') {
      safeDisplay(dailyReserve, 'flex');
      safeDisplay(monthReserve, 'block');
    }
  }
  tabButtons.forEach(btn => btn.addEventListener('click', function () {
    tabButtons.forEach(b => b.classList.remove('active'));
    this.classList.add('active');
    updateFormFields(this.dataset.tab);
  }));
  updateFormFields('daily');

  document.querySelectorAll('.location-input').forEach(input => {
    const wrapper = input.closest('.location-wrapper');
    if (!wrapper) return;
    const dropdown = wrapper.querySelector('.mega-location-dropdown');
    const branches = wrapper.querySelectorAll('.mega-branch-item');
    const categories = wrapper.querySelectorAll('.mega-category');
    if (!dropdown) return;

    input.addEventListener('click', e => {
      e.stopPropagation();
      document.querySelectorAll('.mega-location-dropdown').forEach(d => d.style.display = 'none');
      dropdown.style.display = 'block';
    });

    categories.forEach(cat => {
      cat.addEventListener('click', () => {
        categories.forEach(c => c.classList.remove('active'));
        cat.classList.add('active');
      });
    });

    branches.forEach(branch => {
      branch.addEventListener('mouseenter', () => showBranchInfo(branch));
      branch.addEventListener('click', () => {
        branches.forEach(b => b.classList.remove('active'));
        branch.classList.add('active');
        input.value = branch.dataset.name || branch.textContent.trim();
        showBranchInfo(branch);
        dropdown.style.display = 'none';
      });
    });

    function showBranchInfo(branch) {
      const infoBox = wrapper.querySelector('.mega-info');
      if (!infoBox) return;
      const hours = infoBox.querySelector('#branchHours');
      const location = infoBox.querySelector('#branchLocation');
      if (hours) hours.textContent = branch.dataset.hours || 'غير متوفر';
      if (location) location.textContent = branch.dataset.location || 'غير متوفر';
    }
  });
  document.addEventListener('click', e => { if (!e.target.closest('.location-wrapper')) document.querySelectorAll('.mega-location-dropdown').forEach(d => d.style.display = 'none'); });

  if (typeof flatpickr !== "undefined") {
    const pickupInput = document.getElementById("pickup-date");
    const returnInput = document.getElementById("return-date");
    const timeInput = document.getElementById("pickup-time");

    const pickupPicker = pickupInput ? flatpickr(pickupInput, { dateFormat: "d/m/Y", minDate: "today", disableMobile: true, locale: currentLang === "ar" ? "ar" : "en" }) : null;
    const returnPicker = returnInput ? flatpickr(returnInput, { dateFormat: "d/m/Y", minDate: "today", disableMobile: true, locale: currentLang === "ar" ? "ar" : "en" }) : null;
    const timePicker = timeInput ? flatpickr(timeInput, { enableTime: true, noCalendar: true, dateFormat: "H:i", time_24hr: true, disableMobile: true, locale: currentLang === "ar" ? "ar" : "en" }) : null;

    document.addEventListener("click", e => {
      if (!e.target.closest(".flatpickr-calendar") && !e.target.closest("#pickup-date") && !e.target.closest("#return-date") && !e.target.closest("#pickup-time")) {
        pickupPicker?.close();
        returnPicker?.close();
        timePicker?.close();
      }
    });
  }

  initSwipers();

  const bookingForm = document.querySelector('.booking-form');
  const carsSection = document.getElementById('carsSection');

  bookingForm?.addEventListener('submit', function (e) {
    e.preventDefault();
    const pickup = document.getElementById('pickupLocation')?.value.trim();
    const dropoff = document.getElementById('dropoffLocation')?.value.trim();
    const pickupDate = document.getElementById('pickup-date')?.value.trim();
    const returnDate = document.getElementById('return-date')?.value.trim();

    if (!pickup || !dropoff || !pickupDate || !returnDate) {
      alert(currentLang === "ar" ? "يرجى ملء جميع الحقول" : "Please fill all fields");
      return;
    }

    carsSection.classList.remove('d-none');
    carsSection.classList.add('show');
    carsSection.scrollIntoView({ behavior: "smooth" });
  });

  const categoryFilter = document.getElementById("categoryFilter");
  const priceSort = document.getElementById("priceSort");
  const availableOnly = document.getElementById("availableOnly");
  const carsContainer = document.querySelector(".cars-section .row");
  const carItems = document.querySelectorAll(".car-item");

  function filterCars() {
    let carsArray = Array.from(carItems);
    const selectedCategory = categoryFilter?.value || "all";
    const selectedSort = priceSort?.value || "";
    const showAvailable = availableOnly?.checked || false;

    carsArray.forEach(car => {
      const category = car.dataset.category;
      const available = car.dataset.available === "true";
      let show = true;
      if (selectedCategory !== "all" && category !== selectedCategory) show = false;
      if (showAvailable && !available) show = false;
      car.style.display = show ? "block" : "none";
    });

    if (selectedSort && carsContainer) {
      let visibleCars = carsArray.filter(car => car.style.display !== "none");
      visibleCars.sort((a, b) => {
        let priceA = parseFloat(a.dataset.price) || 0;
        let priceB = parseFloat(b.dataset.price) || 0;
        return selectedSort === "low" ? priceA - priceB : priceB - priceA;
      });
      visibleCars.forEach(car => carsContainer.appendChild(car));
    }
  }

  categoryFilter?.addEventListener("change", filterCars);
  priceSort?.addEventListener("change", filterCars);
  availableOnly?.addEventListener("change", filterCars);

  const observer = new MutationObserver(mutations => {
    mutations.forEach(mutation => {
      if (mutation.attributeName === 'dir') setTimeout(initSwipers, 50);
    });
  });
  observer.observe(document.documentElement, { attributes: true });

  switchLanguage(currentLang);
});
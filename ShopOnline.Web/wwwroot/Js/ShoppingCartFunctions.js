function MakeUpdateQtyButtonVisible(Id, visible)
{
    const updateQtyButton = document.querySelector("button[data-itemId='" + Id + "']");

    if (visible === true) {
        updateQtyButton.style.display = "inline-block";
    }
    else {
        updateQtyButton.style.display = "none";
    }
}
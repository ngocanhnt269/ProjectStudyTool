// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var currentCardIndex = 0;
var flashcards = document.querySelectorAll(".flashcard");

function toggleFlashcard() {
  var currentFlashcard = flashcards[currentCardIndex];
  var question = currentFlashcard.querySelector(".question");
  var answer = currentFlashcard.querySelector(".answer");

  if (question.style.display === "none") {
    question.style.display = "block";
    answer.style.display = "none";
  } else {
    question.style.display = "none";
    answer.style.display = "block";
  }
}

function nextFlashcard() {
  console.log("Next flashcard called");
  if (currentCardIndex < flashcards.length - 1) {
    toggleFlashcard();
    currentCardIndex++;
    toggleFlashcard();
  }
}

function previousFlashcard() {
  console.log("Previous flashcard called");
  if (currentCardIndex > 0) {
    toggleFlashcard();
    currentCardIndex--;
    toggleFlashcard();
  }
}

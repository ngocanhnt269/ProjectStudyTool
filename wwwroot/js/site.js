var currentCardIndex = 0;
var flashcards = window.flashcards || [];

function displayFlashcard(index) {
  var flashcard = flashcards[index];
  var questionElement = document.querySelector("#flashcard .question");
  var answerElement = document.querySelector("#flashcard .answer");

  questionElement.textContent = flashcard.question;
  answerElement.textContent = flashcard.answer;

  // Initially show the question and hide the answer
  questionElement.style.display = "block";
  answerElement.style.display = "none";
}

function toggleFlashcard() {
  var questionElement = document.querySelector("#flashcard .question");
  var answerElement = document.querySelector("#flashcard .answer");

  // If the answer is hidden, show it and hide the question
  if (answerElement.style.display === "none") {
    answerElement.style.display = "block";
    questionElement.style.display = "none";
  }
  // If the answer is shown, hide it and show the question
  else {
    answerElement.style.display = "none";
    questionElement.style.display = "block";
  }
}

function nextFlashcard(callback) {
  console.log("Next flashcard called");
  if (currentCardIndex < flashcards.length - 1) {
    currentCardIndex++;
    displayFlashcard(currentCardIndex);
  }
  callback();
}

function previousFlashcard(callback) {
  console.log("Previous flashcard called");
  if (currentCardIndex > 0) {
    currentCardIndex--;
    displayFlashcard(currentCardIndex);
  }
  callback();
}

// Display the first flashcard initially
displayFlashcard(currentCardIndex);

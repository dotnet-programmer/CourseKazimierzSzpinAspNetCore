namespace GymManager.Application.Common.Events;

// eventy
// 1. klasa - marker do oznaczania eventów
// dzięki temu można oznaczyć w metodzie do publikowania eventów, że oczekujemy że parametr będzie tego typu
// będzie to również pomocne do rejestrowania interfejsów w kontenerze Dependency Injection za pomocą refleksji
public interface IEvent { }
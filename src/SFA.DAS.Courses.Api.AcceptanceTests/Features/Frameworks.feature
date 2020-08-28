Feature: Frameworks
	As a Courses API consumer
	I want to retrieve frameworks
	So that I can use them in my own application

Scenario: Get list of frameworks
	Given I have an http client
	When I GET the following url: api/courses/frameworks
	Then an http status code of 200 is returned
    And all frameworks are returned

Scenario: Get framework by id
    Given I have an http client
    When I GET the following url: api/courses/frameworks/1
    Then an http status code of 200 is returned
    And the framework with id equal to 1 is returned
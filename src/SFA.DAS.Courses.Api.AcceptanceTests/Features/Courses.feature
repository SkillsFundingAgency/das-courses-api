Feature: Courses
    As a Courses API consumer
    I want to retrieve courses
    So that I can use them in my own application

    Scenario: Get list of active and available courses
    Given I have an http client
    When I GET the following url: /api/courses/search?filter=ActiveAvailable
    Then an http status code of 200 is returned
    And all valid courses are returned

    Scenario: Get list of courses by keyword that are active and available
    Given I have an http client
    When I GET the following url: /api/courses/search?keyword=beer&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title       | level | route                         | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Head Brewer | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |
    | Beer Taster | 5     | Creative and design           | 1.0     | Approved for delivery |

    Scenario: Get list of courses by keyword with none filter
    Given I have an http client
    When I GET the following url: /api/courses/search?filter=None&keyword=beer
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title                                      | level | route                         | version | status                 |
    | Brewer                                     | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Assistant Brewer - Proposal in development | 1     | Engineering and manufacturing | 1.1     | Proposal in development|
    | Assistant Brewer - Withdrawn               | 1     | Engineering and manufacturing | 1.0     | Withdrawn              |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.2     | Retired                |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.1     | Retired                |
    | Beer Taster                                | 5     | Creative and design           | 1.0     | Approved for delivery |

    Scenario: Get list of courses by keyword
    Given I have an http client
    When I GET the following url: /api/courses/search?keyword=beer
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title       | level | route                         | version | status                 |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Head Brewer | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Beer Taster | 5     | Creative and design           | 1.0     | Approved for delivery |
    
    Scenario: Get list of courses by levels that are active and available
    Given I have an http client
    When I GET the following url: /api/courses/search?levels=1&levels=7&levels=5&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title       | level | route                         | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Dentist     | 7     | Construction                  | 1.0     | Approved for delivery |
    | Beer Taster | 5     | Creative and design           | 1.0     | Approved for delivery |

    Scenario: Get list of courses by routes that are active and available
    Given I have an http client
    When I GET the following url: /api/courses/search?routeIds=2&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title                                           | level | route       | version | status                |
    | Dentist                                         | 7     | Construction | 1.0     | Approved for delivery |
    | Senior / head of facilities management (degree) | 6     | Construction | 1.0     | Approved for delivery |

    Scenario: Get list of courses by keyword and levels that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/search?keyword=beer&levels=1&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title       | level | route                        | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |

    Scenario: Get list of courses by keyword sorted by relevance that are active and available
    Given I have an http client
    When I GET the following url: /api/courses/search?keyword=sortorder&orderBy=score&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title                            | level | route              | version | status                |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |

    Scenario: Get list of courses by keyword sorted by name that are active and available
    Given I have an http client
    When I GET the following url: /api/courses/search?keyword=sortorder&orderBy=title&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid courses are returned
    | title                            | level | route              | version | status                |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |

    Scenario: Get list of active courses not restricted by start date
    Given I have an http client
    When I GET the following url: /api/courses/search?filter=Active
    Then an http status code of 200 is returned
    And all valid and invalid courses are returned

    Scenario: Get list of all courses
    Given I have an http client
    When I GET the following url: /api/courses/search?filter=None
    Then an http status code of 200 is returned
    And all courses are returned

    Scenario: Get list the default list of courses
    Given I have an http client
    When I GET the following url: /api/courses/search
    Then an http status code of 200 is returned
    And all valid courses are returned

    Scenario: Get list of not yet approved courses
    Given I have an http client
    When I GET the following url: /api/courses/search?filter=NotYetApproved
    Then an http status code of 200 is returned
    And all not yet approved courses are returned

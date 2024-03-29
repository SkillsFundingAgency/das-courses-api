﻿Feature: Standards
	As a Courses API consumer
    I want to retrieve standards
    So that I can use them in my own application

    Scenario: Get list of active and available standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=ActiveAvailable
    Then an http status code of 200 is returned
    And all valid standards are returned

    Scenario: Get list of standards by keyword that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | route                        | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Head Brewer | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |

    Scenario: Get list of standards by keyword with none filter
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=None&keyword=beer
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | route                        | version | status                 |
    | Brewer                                     | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Assistant Brewer - Proposal in development | 1     | Engineering and manufacturing | 1.1     | Proposal in development|
    | Assistant Brewer - Withdrawn               | 1     | Engineering and manufacturing | 1.0     | Withdrawn              |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.2     | Retired                |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.1     | Retired                |

    Scenario: Get list of standards by keyword
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | route                        | version | status                 |
    | Brewer                                     | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    
    Scenario: Get list of standards by levels that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?levels=1&levels=7&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title   | level | route                        | version | status                |
    | Brewer  | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Dentist | 7     | Construction                  | 1.0     | Approved for delivery |

    Scenario: Get list of standards by routes that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?routeIds=2&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                           | level | route       | version | status                |
    | Dentist                                         | 7     | Construction | 1.0     | Approved for delivery |
    | Senior / head of facilities management (degree) | 6     | Construction | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword and levels that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&levels=1&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | route                        | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword sorted by relevance that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=score&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | route              | version | status                |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword sorted by name that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=title&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | route              | version | status                |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |

    Scenario: Get list of active standards not restricted by start date
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=Active
    Then an http status code of 200 is returned
    Then all valid and invalid standards are returned

    Scenario: Get list of all standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?Filter=None
    Then an http status code of 200 is returned
    And all standards are returned

    Scenario: Get list the default list of standards
	Given I have an http client
    When I GET the following url: /api/courses/standards
    Then an http status code of 200 is returned
    And all valid standards are returned

    Scenario: Get list of not yet approved standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=NotYetApproved
    Then an http status code of 200 is returned
    And all not yet approved standards are returned

    Scenario: Get latest version of a standard by Lars Code
	Given I have an http client
    When I GET the following url: /api/courses/standards/1
    Then an http status code of 200 is returned
    And the following standard is returned
    | title                            | level | route                        | version | status                |
    | Head Brewer                      | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |

    Scenario: Get latest version of a standard by IfateReferenceNumber
	Given I have an http client
    When I GET the following url: /api/courses/standards/ST0001
    Then an http status code of 200 is returned
    And the following standard is returned
    | title                            | level | route                        | version | status                |
    | Head Brewer                      | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |
    
    Scenario: : Get a specific version of a standard by StandardUId
    Given I have an http client
    When I GET the following url: /api/courses/standards/ST0005_1.0
    Then an http status code of 200 is returned
    And the following standard is returned
    | title                            | level | route              | version | status  |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.0     | Retired |

    Scenario: : Get a all versions of a standard by IFateReferenceNumber
    Given I have an http client
    When I GET the following url: /api/courses/standards/versions/ST0001
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | route                        | version | status                 |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.2     | Retired                |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.1     | Retired                |

    Scenario: Get KSBs for a standard's option
    Given I have an http client
    When I GET the following url: /api/courses/standards/8/options/Option 1/ksbs
    Then an http status code of 200 is returned
    And the following knowledges are returned
    | Type      | Key | Detail           |
    | Knowledge | K1  | core_knowledge_1 |
    | Knowledge | K2  | core_knowledge_2 |
    | Knowledge | K3  | opt1_knowledge_3 |
    | Skill     | S1  | core_skill_1     |
    | Behaviour | B1  | opt1_behaviour_1 |

    Scenario: A standard with no options
	Given I have an http client
    When I get a standard with a core pseudo-option
    Then an http status code of 200 is returned
    And the returned standard has no options

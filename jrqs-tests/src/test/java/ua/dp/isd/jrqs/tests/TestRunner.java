package ua.dp.isd.jrqs.tests;

import java.time.Duration;
import org.junit.runner.JUnitCore;
import org.junit.runner.Result;

public class TestRunner {

    public static void main(String[] args) {
        Result result = JUnitCore.runClasses(TestSuite.class);

        result.getFailures().forEach((failure) -> {
            System.out.println(failure.toString());
        });

        System.out.println(String.format("Tests have been executed in %s seconds.", Duration.ofMillis(result.getRunTime()).getSeconds()));

        if (result.wasSuccessful()) {
            System.out.println("All tests have been passed successfully!");
        } else {
            System.out.println("Some tests have been failed!");
        }
    }
}

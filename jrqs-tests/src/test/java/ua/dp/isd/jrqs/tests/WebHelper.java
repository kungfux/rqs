package ua.dp.isd.jrqs.tests;

import java.time.Duration;
import java.util.List;
import java.util.concurrent.TimeUnit;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.ie.InternetExplorerDriver;

public class WebHelper {

    private static WebDriver _driver;
    private final Duration TIMEOUT = Duration.ofSeconds(10);
    
    public WebHelper() {
        if (_driver != null)
            return;
        _driver = new InternetExplorerDriver();
        enableTimeout();
    }

    public WebDriver getDriver() {
        return _driver;
    }

    public WebElement findElementByCss(String cssSelector) {
        enableTimeout();
        return _driver.findElement(By.cssSelector(cssSelector));
    }
    
    public WebElement findElementByCssWithoutAwaiting(String cssSelector) {
        disableTimeout();
        return _driver.findElement(By.cssSelector(cssSelector));
    }

    public List<WebElement> findElementsByCss(String cssSelector) {
        enableTimeout();
        return _driver.findElements(By.cssSelector(cssSelector));
    }
    
    public List<WebElement> findElementsByCssWithoutAwaiting(String cssSelector) {
        disableTimeout();
        return _driver.findElements(By.cssSelector(cssSelector));
    }
    
    private void enableTimeout() {
        setTimeout(TIMEOUT.getSeconds());
    }
    
    private void disableTimeout() {
        setTimeout(Duration.ZERO.getSeconds());
    }
    
    private void setTimeout(long seconds) {
        _driver.manage().timeouts().implicitlyWait(seconds, TimeUnit.SECONDS);
    }
}
